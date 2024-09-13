using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAR {
	//private static readonly string InsertRcDokumentas = "INSERT INTO raw_rc_dokumentai (dok_id,dok_kodas,dok_nr,dok_pastabos,dok_data,dok_data_pateik,dok_data_reg,dok_tipas,dok_potipis,dok_notaro_nr,dok_anul) VALUES (@id,@kodas,@nr,@pastabos,@data,@datapateik,@datareg,@tipas,@potipis,@notaronr,@anul);";


	public static class DB {
		public static string ConnStr { private get; set; } = "User ID=postgres; Password=postgres; Server=localhost:5432; Database=master;";
		public static async Task<int> Execute(string sql) {
			var conn = new NpgsqlConnection(ConnStr); await conn.OpenAsync();
			return await new NpgsqlCommand(sql, conn).ExecuteNonQueryAsync();
		}
		public static DBBulk Bulk(string table, List<string> fld) => new (table, fld, DB.ConnStr);
    }

	public class DBBulk : IDisposable {
        public List<string> Fields { get; }
		private NpgsqlConnection Conn { get; set; }
		private NpgsqlBinaryImporter Imp { get; set; }
		public void Insert(object?[] row) { lock (Imp) Imp.WriteRow(row); }
		public void Complete() { Imp.Complete(); }
        public DBBulk(string table, List<string> fld, string conn) {
			Fields = fld; Conn = new NpgsqlConnection(conn); Conn.Open();
			Imp = Conn.BeginBinaryImport($"COPY {table} ({string.Join(", ", Fields)}) FROM STDIN (FORMAT BINARY)");
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
