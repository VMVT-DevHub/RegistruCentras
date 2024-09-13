
using System.Xml;
using RC.Extensions;

namespace RC.Classes;

public class RowSet : ILoadXML {
	public RowKlasif? Klasifikatorius { get; set; }
	public RowAtributas? Atributas { get; set; }
	void ILoadXML.LoadData(string key, XmlReader rdr){
		switch(key){
			case "KLA_KODAS": (Klasifikatorius??=new()).Kodas=rdr.Next().Value; break;
			case "KLA_APRASYMAS": (Klasifikatorius??=new()).Aprasymas=rdr.Next().Value; break;
			case "KLA_XML_DATA": (Klasifikatorius??=new()).Data=rdr.Next().ValDateTime(); break;		

			case "DOKI_ISTAIGOS_ID": case "NJA_ID": (Atributas??=new()).ID=rdr.Next().ValInt()??0; break;
			case "REIK_REIKSME": case "VEIK_KODAS_STR": case "SAL_TRIRAIDIS_KODAS": (Atributas??=new()).Reiksme=rdr.Next().Value; break;
			case "FAKT_TIPAS": case "DOKT_TIPAS": case "VEIK_VERSIJA": (Atributas??=new()).Tipas=rdr.Next().ValInt()??0; break;
			case "FAKP_POTIPIS": case "DOKP_POTIPIS": (Atributas??=new()).Potipis=rdr.Next().ValInt()??0; break;
			case "FORM_GALIOJA_IKI": (Atributas??=new()).GaliojaIki=rdr.Next().ValDateTime(); break;

			case "VEIK_KODAS": case "TATR_KODAS": case "STAT_STATUSAS": case "SAL_KODAS": case "PAR_KODAS": 
			case "NJA_KODAS": case "VIEN_KODAS": case "ISPR_KODAS": case "FORM_KODAS": case "DBUK_KODAS": 
			case "DOKI_KODAS": case "ATRI_KODAS": (Atributas??=new()).Kodas=rdr.Next().ValInt()??0; break;

			case "VEIK_PAVADINIMAS": case "TATR_PAV_I":  case "STAT_PAV_I": case "SAL_PAV": 
			case "PAR_PAV_ILGAS": case "NJA_PAVADINIMAS": case "VIEN_PAV": case "ISPR_PAVAD_I": 
			case "FORM_PAV_I": case "FAKT_PAV_I": case "FAKP_PAV_I": case "DBUK_PAV_I": case "DOKT_PAV": 
			case "DOKP_PAV_I": case "DOKI_PAVAD": case "REIK_PAV_I": 
			case "ATRI_PAV_I": (Atributas??=new()).Pavadinimas=rdr.Next().Value; break;

			case "VEIK_PAV_ENGLISH": case "TATR_PAV_ENGLISH": case "STAT_PAV_ENGLISH": case "SAL_PAV_ENGLISH": 
			case "PAR_PAV_ENGLISH": case "VIEN_PAV_ENGLISH": case "ISPR_PAVAD_ENGLISH": case "FORM_PAV_ENGLISH": 
			case "FAKT_PAV_ENGLISH": case "FAKP_PAV_ENGLISH": case "DBUK_PAV_ENGLISH": case "DOKT_PAV_ENGLISH": 
			case "DOKP_PAV_ENGLISH": case "REIK_PAV_ENGLISH": 
			case "ATRI_PAV_ENGLISH": (Atributas??=new()).PavadinimasEn=rdr.Next().Value; break;

			case "VEIK_KOREGAVIMO_DATA": case "TATR_KOREGAVIMO_DATA": case "STAT_KOREGAVIMO_DATA": case "SAL_KOREGAVIMO_DATA": 
			case "PAR_KOREGAVIMO_DATA": case "NJA_KOREGAVIMO_DATA": case "VIEN_KOREGAVIMO_DATA": case "ISPR_KOREGAVIMO_DATA": 
			case "FORM_KOREGAVIMO_DATA": case "FAKT_KOREGAVIMO_DATA": case "FAKP_KOREGAVIMO_DATA": case "DBUK_KOREGAVIMO_DATA": 
			case "DOKT_KOREGAVIMO_DATA": case "DOKP_KOREGAVIMO_DATA": case "DOKI_KOREGAVIMO_DATA": case "REIK_KOREGAVIMO_DATA": 
			case "ATRI_KOREGAVIMO_DATA": (Atributas??=new()).Data=rdr.Next().ValDateTime(); break;
			default: Console.WriteLine($"DataAtributas Missing: {key}"); break;
		}
	}
}

public class RowAtributas : RowSet {		
	public int ID { get; set; }
	public int Kodas { get; set; }
	public int Tipas { get; set; }
	public int Potipis { get; set; }
	public string? Reiksme { get; set; }
	public string? Pavadinimas { get; set; }
	public string? PavadinimasEn { get; set; }
	public DateTime? Data { get; set; }
	public DateTime? GaliojaIki { get; set; }
}

public class RowKlasif : RowSet {
	public string? Kodas { get; set; }
	public string? Aprasymas { get; set; }
	public DateTime? Data { get; set; }
	public List<RowAtributas>? Atributai { get; set; }
}

