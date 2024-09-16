using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAR {


	public static class DB {
		public static string ConnStr { private get; set; } = "User ID=postgres; Password=postgres; Server=localhost:5432; Database=master;";
		public static async Task<int> Execute(string sql) {
			var conn = new NpgsqlConnection(ConnStr); await conn.OpenAsync();
			return await new NpgsqlCommand(sql, conn).ExecuteNonQueryAsync();
		}
		public static DBBulk Bulk(string table, List<string> fld) => new(table, fld, DB.ConnStr);

		public static async Task<NpgsqlDataReader> Read(string sql) {
			var conn = new NpgsqlConnection(ConnStr); await conn.OpenAsync();
			using var command = new NpgsqlCommand(sql, conn);
			return await command.ExecuteReaderAsync();
		}
	}

	public class DBBulk : IDisposable {
		public string Table { get; }
        public List<string> Fields { get; }
		public NpgsqlConnection Conn { get; private set; }
		private NpgsqlBinaryImporter? Imp { get; set; }
		private object Lock { get; set; } = new object();
		private NpgsqlBinaryImporter GetImp => Imp ??= Conn.BeginBinaryImport($"COPY {Table} ({string.Join(", ", Fields)}) FROM STDIN (FORMAT BINARY)");
		public void Insert(object?[] row) { lock (Lock) GetImp.WriteRow(row); }
		public void Insert(object?[] row, NpgsqlDbType?[] type) {
			lock (Lock) {
				GetImp.StartRow();
				for(var i=0; i<row.Length; i++) {
					var r = row[i]; var t = type[i];
					if (r is null) GetImp.WriteNull();
					else if (t is null) GetImp.Write(r);
					else GetImp.Write(r, t.Value);
				}
			}
		}
		public void Complete() { GetImp.Complete(); GetImp.Dispose(); }
		public async Task CompleteAsync() { await GetImp.CompleteAsync(); await GetImp.DisposeAsync(); }
		public DBBulk(string table, List<string> fld, string conn) {
			Table = table; Fields = fld; Conn = new NpgsqlConnection(conn); Conn.Open();
		}

		private bool IsDisposed;
		public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
		protected virtual void Dispose(bool disposing) {
			if (!IsDisposed) {
				if (disposing) { Imp?.Dispose(); Conn?.Dispose(); }
				IsDisposed = true;
			}
		}
	}
}
