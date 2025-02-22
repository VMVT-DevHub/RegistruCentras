
using System.Buffers.Text;
using System.Runtime.Intrinsics.Arm;
using System.Xml;
using Microsoft.Win32;
using RC.Extensions;

namespace RC.Classes;


public class GrResponse : ILoadXML {
	public long AdrAOB { get; set; }
	public long AdrGatve { get; set; }
	public long AdrTer { get; set; }
	public long AK { get; set; }
	public DateOnly? MirtiesData { get; set; }
	public string? MirtiesAktoNr { get; set; }
	public long AKMotinos { get; set; }
	public long AKTevo { get; set; }
	public long AKSutuoktinio { get; set; }
	public string? Nuotrauka { get; set; }
	public string? Parasas { get; set; }
	public int VaikuSkPilnas { get; set; }
	public int VaikuSkaicius { get; set; }
	public string? ErrorCode { get; set; }
	public string? ErrorMessage { get; set; }
    void ILoadXML.LoadAttr(string key, string val, XmlReader rdr){
		switch(key){
			default: Console.WriteLine($"JARItem Attr: {key}: {val}"); break;
		}
	}
	void ILoadXML.LoadData(string key, XmlReader rdr){
		switch (key) {
			case "ats_adr_aob_kodas": AdrAOB = rdr.DataLong() ?? -1; break;
			case "ats_adr_gatve": AdrGatve = rdr.DataLong() ?? -1; break;
			case "ats_adr_ter": AdrTer = rdr.DataLong() ?? -1; break;
			case "ats_asm_kodas": AK = rdr.DataLong() ?? -1; break;
			case "ats_mires": MirtiesData = rdr.DataDate(); break;
			case "ats_mirt_akt_nr": MirtiesAktoNr = rdr.DataString(); break;
			case "ats_mot_kod": AKMotinos = rdr.DataLong() ?? -1; break;
			case "ats_nuotrauka": Nuotrauka = rdr.DataString(); break;
			case "ats_parasas": Parasas = rdr.DataString(); break;
			case "ats_piln_vaik_sk": VaikuSkPilnas = rdr.DataInt() ?? 0; break;
			case "ats_sut_asm_kodas": AKSutuoktinio = rdr.DataLong() ?? -1; break;
			case "ats_tev_kod": AKTevo = rdr.DataLong() ?? -1; break;
			case "ats_vaik_sk": VaikuSkaicius = rdr.DataInt() ?? 0; break;
			case "errorCode": ErrorCode = rdr.DataString(); break;
			case "errorMessage": ErrorMessage = rdr.DataString(); break;
			default: Console.WriteLine($"JARItem Data: {key}: {rdr.DataString()}"); break;
		}
	}
}
