
using Microsoft.Win32;
using RC.Extensions;
using System.Buffers.Text;
using System.Runtime.Intrinsics.Arm;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RC.Classes;


public class GrResponse : ILoadXML {
	public GRResponseAdr? Adresas { get; set; }
	public long AK { get; set; }
	public string? Vardas { get; set; }
	public string? Pavarde { get; set; }
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
			default: Console.WriteLine($"GRItem Attr: {key}: {val}"); break;
		}
	}
	void ILoadXML.LoadData(string key, XmlReader rdr){
		switch (key) {
			case "ats_adr_butas": (Adresas ??= new()).Butas = rdr.DataString(); break;
			case "ats_adr_data_iki": (Adresas ??= new()).DataIki = rdr.DataDate(); break;
			case "ats_adr_data_nuo": (Adresas ??= new()).DataNuo = rdr.DataDate(); break;
			case "ats_adr_dekl": (Adresas ??= new()).Dekl = rdr.DataString(); break;
			case "ats_adr_eil": (Adresas ??= new()).Pavadinimas = rdr.DataString(); break;
			case "ats_adr_gatve": (Adresas ??= new()).Gatve = rdr.DataLong(); break;
			case "ats_adr_korp": (Adresas ??= new()).Korpusas = rdr.DataString(); break;
			case "ats_adr_namas": (Adresas ??= new()).Namas = rdr.DataString(); break;
			case "ats_adr_salis": (Adresas ??= new()).Salis = rdr.DataString(); break;
			case "ats_adr_aob_kodas": (Adresas ??= new()).AOB = rdr.DataLong(); break;
			case "ats_adr_sodas": (Adresas ??= new()).Sodas = rdr.DataString(); break;
			case "ats_adr_ter": (Adresas ??= new()).Teritorija = rdr.DataLong(); break;
			case "ats_adr_tipas": (Adresas ??= new()).Tipas = rdr.DataString(); break;



			case "ats_asm_kodas": AK = rdr.DataLong() ?? -1; break;
			case "ats_asm_pav": Pavarde = rdr.DataString(); break;
			case "ats_asm_vardas": Vardas = rdr.DataString(); break;

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
/*

<xs:element name="ats_asm_gim" type="xs:string" minOccurs="0"/>
<xs:element name="ats_asm_gim_vls" type="xs:string" minOccurs="0"/>
<xs:element name="ats_asm_kodas" type="xs:long"/>
<xs:element name="ats_asm_lytis" type="xs:string" minOccurs="0"/>
<xs:element name="ats_dok_busena" type="xs:string" minOccurs="0"/>
<xs:element name="ats_dok_isd_data" type="xs:string" minOccurs="0"/>
<xs:element name="ats_dok_negal_nuo" type="xs:string" minOccurs="0"/>
<xs:element name="ats_dok_numeris" type="xs:string" minOccurs="0"/>
<xs:element name="ats_dok_tar_kodas" type="xs:string" minOccurs="0"/>
<xs:element name="ats_dok_tipas" type="xs:string" minOccurs="0"/>
<xs:element name="ats_gimviet" type="xs:string" minOccurs="0"/>
<xs:element name="ats_gvna_sav" type="xs:string" minOccurs="0"/>
<xs:element name="ats_ist_data" type="xs:string" minOccurs="0"/>
<xs:element name="ats_isvyk_uzs_data" type="xs:string" minOccurs="0"/>
<xs:element name="ats_klaidingas" type="xs:string" minOccurs="0"/>
<xs:element name="ats_koment" type="xs:string" minOccurs="0"/>
<xs:element name="ats_mires" type="xs:string" minOccurs="0"/>
<xs:element name="ats_mirt_akt_nr" type="xs:long"/>
<xs:element name="ats_mirt_fakt" type="xs:string" minOccurs="0"/>
<xs:element name="ats_mirt_mirtviet" type="xs:string" minOccurs="0"/>
<xs:element name="ats_mot_kod" type="xs:long"/>
<xs:element name="ats_nepiln_vaik_kod" type="xs:string" minOccurs="0"/>
<xs:element name="ats_nuotrauka" type="xs:string" minOccurs="0"/>
<xs:element name="ats_parasas" type="xs:string" minOccurs="0"/>
<xs:element name="ats_pil" type="xs:string" minOccurs="0"/>
<xs:element name="ats_pilietybe_galioja_nuo" type="xs:string" minOccurs="0"/>
<xs:element name="ats_piln_vaik_kod" type="xs:string" minOccurs="0"/>
<xs:element name="ats_piln_vaik_sk" type="xs:int"/>
<xs:element name="ats_sut_asm_kodas" type="xs:long"/>
<xs:element name="ats_sut_busena" type="xs:string" minOccurs="0"/>
<xs:element name="ats_sut_data" type="xs:string" minOccurs="0"/>
<xs:element name="ats_sut_gimdate" type="xs:string" minOccurs="0"/>
<xs:element name="ats_sut_pavarde" type="xs:string" minOccurs="0"/>
<xs:element name="ats_sut_vardas" type="xs:string" minOccurs="0"/>
<xs:element name="ats_tev_kod" type="xs:long"/>
<xs:element name="ats_vaik_sk" type="xs:int"/>
<xs:element name="ats_vaikai" type="personDetailVaikai"/>
<xs:element name="errorCode" type="xs:string" minOccurs="0"/>
<xs:element name="errorMessage" type="xs:string" minOccurs="0"/>
 */


public class GRResponseAdr {
	public string? Butas { get; set; }
	public DateOnly? DataIki { get; set; }
	public DateOnly? DataNuo { get; set; }
	public string? Dekl { get; set; }
	public string? Pavadinimas { get; set; }
	public long? Gatve { get; set; }
	public string? Korpusas { get; set; }
	public string? Namas { get; set; }
	public string? Salis { get; set; }
	public long? AOB { get; set; }
	public string? Sodas { get; set; }
	public long? Teritorija { get; set; }
	public string? Tipas { get; set; }
}
