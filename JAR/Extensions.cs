using RC.Classes;
using System.Reflection.Metadata;
using System.Text;

namespace JAR {
	public static class Extensions {
		public static FaktNaud Parse(this JaFaktNaud fkt) {
			var ret = new FaktNaud() { ID = fkt.ID, JungtNuosav = fkt.JungtNuosavybe, Aprasymas = fkt.Aprasymas, Par = fkt.Par, DataNuo = fkt.DataNuo, DataIki = fkt.DataIki, DataPradzios = fkt.DataPradzios, DataPabaigos = fkt.DataPabaigos, SalKodas = fkt.SalKodas };
			if (fkt.Dokumentai is not null) { var doc = new List<long>(); foreach (var i in fkt.Dokumentai) doc.Add(i.ID); ret.Dokumentai = doc; }
			return ret;
		}
	}

	public class FaktNaud {
		public long ID { get; set; }
		public int? JungtNuosav { get; set; }
		public int? Par { get; set; }
		public string? Aprasymas { get; set; }
		public DateOnly? DataNuo { get; set; }
		public DateOnly? DataIki { get; set; }
		public DateOnly? DataPradzios { get; set; }
		public DateOnly? DataPabaigos { get; set; }
		public List<long>? Dokumentai { get; set; }
		public string? SalKodas { get; set; }
	}
	class OutputLog : TextWriter {
		private readonly TextWriter _innerWriter;
		public OutputLog(TextWriter innerWriter) { _innerWriter = innerWriter; }
		public override Encoding Encoding => throw new NotImplementedException();
		public override void Write(char value) {
			_innerWriter.Write(value);
		}
		public override void Write(string? value) {
			_innerWriter.Write(value);
		}
	}
}
