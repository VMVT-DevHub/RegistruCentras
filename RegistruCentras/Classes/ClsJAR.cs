
using System.Runtime.Intrinsics.Arm;
using System.Xml;
using Microsoft.Win32;
using RC.Extensions;

namespace RC.Classes;

public class JaResponse : ILoadXML {
	public long Kodas => Objektai?.Kodas??0;
	public string? Pavadinimas => Objektai?.Pavadinimas;
	public JaItem? Objektai { get; set; }
	public List<JaDok>? Dokumentai { get; set; }
	public List<JaFizAsm>? FiziniaiAsmenys { get; set; }
	public List<JaFizNeid>? FiziniaiNeid { get; set; }
	public List<JaUzsSteig>? UzsienioSteigejai { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr){}
	void ILoadXML.LoadData(string key, XmlReader rdr){
		switch(rdr.LocalName){
			case "OBJEKTAI": Objektai=rdr.Fill<JaItem>(); break;
			case "DOKUMENTAI": (Dokumentai??=[]).Add(rdr.Fill<JaDok>()); break;
			case "FIZINIAI_ASMENYS": (FiziniaiAsmenys ??= []).Add(rdr.Fill<JaFizAsm>()); break;
			case "FIZINIAI_ASMENYS_NEID": (FiziniaiNeid ??= []).Add(rdr.Fill<JaFizNeid>()); break;
			case "UZSIENIO_STEIGEJAI": (UzsienioSteigejai ??= []).Add(rdr.Fill<JaUzsSteig>()); break;
			default: Console.WriteLine($"JAResponse Missing: {rdr.LocalName}"); break;
		}
	}
	public JaResponse(){} public JaResponse(JaItem ja){ Objektai=ja; }
}

public class JaItem : ILoadXML {
	public long ID { get; set; }
	public long Kodas { get; set; }
	public string? Pavadinimas { get; set; }
	public string? Pastabos { get; set; }
	public string? Komentaras { get; set; }
	public long? PristDezAdr { get; set; }
	public int? Forma { get; set; }
	public long? NTRKodas { get; set; }
	public int? DBUKKodas { get; set; }
	public int? OTIPKodas { get; set; }
	public int? REJEKodas { get; set;}
	public DateOnly? DataReg { get; set; }
	public DateOnly? DataIsreg { get; set; }
    public DateOnly? DataSteigimo { get; set; }
    public int? Anul { get; set; }
	public string? Tikslai { get; set; }
	public int? Pagrindinis { get; set; }
	public int? Versija { get; set; }
	public DateOnly? VersData { get; set; }
	public int? PagrID { get; set; }
	public long? PagrKodas { get; set; }
	public string? PagrPavadinimas { get; set; }

	public List<JaForma>? Formos { get; set; }
	public List<JaAtributas>? Atributai { get; set; }
	public List<JaStatusas>? Statusai { get; set; }
	public List<JaVeikla>? Veiklos { get; set; }
	public List<JaPavadinimas>? Pavadinimai { get; set; }
	public List<JaAdresas>? Adresai { get; set; }
	public List<JaText>? TekstiniaiDuomenys { get; set; }
	public List<JaFaktai>? Faktai { get; set; }
	public List<JaFinAts>? FinansineAtskaitomybe { get; set; }
	public List<JaFilialas>? Filialai { get; set; }
	public List<JaNTObj>? NTObjektai { get; set; }
	public List<JaObjAsm>? ObjektuAsmenys { get; set; }
	public HashSet<long>? DokumentaiID { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr){
		switch(key){
			case "OBJ_ID": ID=rdr.ValLong()??-1; break;
			case "OBJ_KODAS": Kodas=rdr.ValLong()??-1; break;
			case "OBJ_PAV": Pavadinimas=val; break;
			case "OBJ_PASTABOS": Pastabos=val; break;
			case "OBJ_KOMENTARAS": Komentaras=val; break;
			case "JA_E_PRIST_DEZUTES_ADR": PristDezAdr=rdr.ValLong(); break;
			case "FORM_KODAS": Forma=rdr.ValInt(); break;
			case "OBJ_NTR_KODAS": NTRKodas=rdr.ValLong(); break;
			case "DBUK_KODAS": DBUKKodas=rdr.ValInt(); break;
			case "OTIP_KODAS": OTIPKodas=rdr.ValInt(); break;
			case "OBJ_REJESTRO_KODAS": REJEKodas=rdr.ValInt(); break;
			case "OBJ_REG_DATA": DataReg=rdr.ValDate(); break;
			case "OBJ_ISREG_DATA": DataIsreg=rdr.ValDate(); break;
			case "OBJ_STEIGIMO_DATA": DataSteigimo=rdr.ValDate(); break;
			case "OBJ_ANUL": Anul=rdr.ValInt(); break;
			case "OBJ_TIKSLAI": Tikslai=val; break;
			case "OBJ_PAGRINDINIS": Pagrindinis=rdr.ValInt(); break;
			case "VER_VERSIJA": Versija=rdr.ValInt(); break;
			case "VER_DATA_NUO": VersData=rdr.ValDate(); break;
			case "OBJ_ID_PRIKLAUSO": PagrID=rdr.ValInt(); break;
			case "PAGR_OBJ_KODAS": PagrKodas=rdr.ValLong(); break;
			case "PAGR_OBJ_PAV": PagrPavadinimas=val; break;
			default: Console.WriteLine($"JARItem Attr: {key}: {val}"); break;
		}
	}
	void ILoadXML.LoadData(string key, XmlReader rdr){
		switch(key){
			case "JA_FORMOS": (Formos??=[]).Add(rdr.Fill<JaForma>()); break;
			case "OBJEKTU_ATRIBUTAI": (Atributai??=[]).Add(rdr.Fill<JaAtributas>()); break;
			case "JA_STATUSAI": (Statusai??=[]).Add(rdr.Fill<JaStatusas>()); break;
			case "JA_VEIKLOS": (Veiklos ??= []).Add(rdr.Fill<JaVeikla>()); break;
			case "JA_PAVADINIMAI": (Pavadinimai??=[]).Add(rdr.Fill<JaPavadinimas>()); break;
			case "JA_ADRESAI": (Adresai??=[]).Add(rdr.Fill<JaAdresas>()); break;
			case "TEKSTINIAI_DUOMENYS": (TekstiniaiDuomenys??=[]).Add(rdr.Fill<JaText>()); break;
			case "NTR_OBJEKTAI": (NTObjektai??=[]).Add(rdr.Fill<JaNTObj>()); break;
			case "OBJEKTU_ASMENYS": (ObjektuAsmenys??=[]).Add(rdr.Fill<JaObjAsm>()); break;
			case "FAKTAI": (Faktai??=[]).Add(rdr.Fill<JaFaktai>()); break;
			case "FINANSINE_ATSKAITOMYBE": (FinansineAtskaitomybe ??= []).Add(rdr.Fill<JaFinAts>()); break;
			case "FILIALAI": (Filialai ??= []).Add(rdr.Fill<JaFilialas>()); break;
			case "OBJEKTU_DOKUMENTAI": if(long.TryParse(rdr.GetAttribute("DOK_ID"),out var doc)) (DokumentaiID??=[]).Add(doc); break;
			default: Console.WriteLine($"JARItem Data: {rdr.LocalName}"); break;
		}
	}
}
public class JaForma : ILoadXML {
	public int Kodas { get; set; }
	public DateOnly? DataNuo { get; set; }
	public DateOnly? DataIki { get; set; }
	public int? Anul { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch (key) {
			case "FORM_KODAS": Kodas = rdr.ValInt() ?? -1; break;
			case "JAF_DATA_NUO": DataNuo = rdr.ValDate(); break;
			case "JAF_DATA_IKI": DataIki = rdr.ValDate(); break;
			case "JAF_ANUL": Anul = rdr.ValInt(); break;
			default: Console.WriteLine($"JAForma: {key}: {val}"); break;
		}
	}
}
public class JaAtributas : ILoadXML {
	public int Kodas { get; set; }
	public string? Reiksme { get; set; }
	public DateTime? DataNuo { get; set; }
	public DateOnly? DataIki { get; set; }
	public int? VienKodas { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch(key){
			case "ATRI_KODAS": Kodas=rdr.ValInt()??-1; break;
			case "ATR_REIKSME": Reiksme=val; break;
			case "ATR_DATA_NUO": DataNuo=rdr.ValDateTime(); break;
			case "ATR_DATA_IKI": DataIki=rdr.ValDate(); break;					
			case "VIEN_KODAS": VienKodas=rdr.ValInt(); break;
			default: Console.WriteLine($"JAAtributas: {key}: {val}"); break;
		}
	}
}
public class JaStatusas : ILoadXML { 
	public int Statusas { get; set; }
	public DateOnly? DataIgijimo { get; set; }
	public DateOnly? DataNetekimo { get; set; }
	public DateOnly? DataNuo { get; set; }
	public DateOnly? DataIki { get; set; }
	public int? IsPrKodas { get; set; }
	public int? Anul { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch(key){
			case "STAT_STATUSAS": Statusas=rdr.ValInt()??-1; break;					
			case "JST_IGIJIMO_DATA": DataIgijimo=rdr.ValDate(); break;
			case "JST_NETEKIMO_DATA": DataNetekimo=rdr.ValDate(); break;
			case "JST_DATA_NUO": DataNuo=rdr.ValDate(); break;
			case "JST_DATA_IKI": DataIki=rdr.ValDate(); break;					
			case "ISPR_KODAS": IsPrKodas=rdr.ValInt(); break;	
			case "JST_ANUL": Anul=rdr.ValInt(); break;	
			default: Console.WriteLine($"JAStatusas: {key}: {val}"); break;
		}
	}
}
public class JaVeikla : ILoadXML {
	public int Kodas { get; set; }
	public int? Versija { get; set; }
	public DateTime? DataNuo { get; set; }
	public DateOnly? DataIki { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch (key) {
			case "VEIK_KODAS": Kodas = rdr.ValInt() ?? -1; break;
			case "VEIK_VERSIJA": Versija = rdr.ValInt(); break;
			case "VEI_DATA_NUO": DataNuo = rdr.ValDateTime(); break;
			case "VEI_DATA_IKI": DataIki = rdr.ValDate(); break;
			default: Console.WriteLine($"JAStatusas: {key}: {val}"); break;
		}
	}
}
public class JaPavadinimas : ILoadXML { 
	public int Kodas { get; set; }
	public string? RegNr { get; set; }
	public DateOnly? DataReg { get; set; }
	public DateTime? DataNuo { get; set; }
	public DateOnly? DataIki { get; set; }
	public string? Pavadinimas { get; set; }
	public int? Anul { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch(key){
			case "PTIP_KODAS": Kodas=rdr.ValInt()??-1; break;
			case "PAV_REG_NR": RegNr=val; break;
			case "PAV_REG_DATA": DataReg=rdr.ValDate(); break;
			case "PAV_DATA_NUO": DataNuo=rdr.ValDateTime(); break;
			case "PAV_DATA_IKI":DataIki =rdr.ValDate(); break;
			case "PAV_PAV": Pavadinimas=val; break;
			case "PAV_ANUL": Anul=rdr.ValInt(); break;
			default: Console.WriteLine($"JAPavadinimas: {key}: {val}"); break;
		}
	}
}
public class JaAdresas : ILoadXML {
	public DateTime? DataNuo { get; set; }
	public DateOnly? DataIki { get; set; }
	public string? Pavadinimas { get; set; }
	public long? AOB { get; set; }
	public int? AdrBus { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch(key){
			case "JAD_DATA_NUO": DataNuo=rdr.ValDateTime(); break;
			case "JAD_DATA_IKI": DataIki=rdr.ValDate(); break;
			case "JAD_TEKSTAS": Pavadinimas=val; break;
			case "ARO_KODAS": AOB=rdr.ValLong(); break;
			case "ADR_BUS": AdrBus=rdr.ValInt(); break;
			default: Console.WriteLine($"JAAdresas: {key}: {val}"); break;
		}
	}
}
public class JaText : ILoadXML {
	public int Kodas { get; set; }
	public DateTime? DataNuo { get; set; }
	public DateOnly? DataIki { get; set; }
	public string? Tekstas { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch(key){
			case "TATR_KODAS": Kodas=rdr.ValInt()??-1; break;
			case "TXD_DATA_NUO": DataNuo=rdr.ValDateTime(); break;
			case "TXD_GALIOJA_IKI": DataIki=rdr.ValDate(); break;
			case "TXD_TEKSTAS": Tekstas=val; break;
			default: Console.WriteLine($"JAText: {key}: {val}"); break;
		}
	}
}
public class JaNTObj : ILoadXML {
	public long NTR { get; set; }
	public DateTime? DataNuo { get; set; }
	public DateOnly? DataIki { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch(key){
			case "NTR_KODAS": NTR=rdr.ValLong()??-1; break;
			case "NTR_DATA_NUO": DataNuo=rdr.ValDateTime(); break;
			case "NTR_DATA_IKI": DataIki=rdr.ValDate(); break;
			default: Console.WriteLine($"JANTObj: {key}: {val}"); break;
		}
	}
}
public class JaObjAsm : ILoadXML {
	public long ID { get; set; }
	public long? FaId { get; set; }
	public long? JaId { get; set; }
	public long? Kodas { get; set; }
	public string? Pavadinimas { get; set; }
	public string? Adresas { get; set; }
	public string? JADTekstas { get; set; } //new
	public long? AOB { get; set; }
	public int? AdrBus { get; set; }
	public long? NTRId { get; set; } //new
	public string? NTRKodas { get; set; } //new
	public long? NeidId { get; set; }
    public long? NjaId { get; set; }
	public int? SteId { get; set; }

	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch(key){
			case "OBA_ID": ID=rdr.ValLong()??-1; break;
			case "FIZ_ID": FaId=rdr.ValLong(); break;
			case "NEID_FIZ_ID": NeidId=rdr.ValLong(); break;
			case "OBJ_ID_ASM": JaId=rdr.ValLong(); break;
			case "OBJ_KODAS": Kodas=rdr.ValLong(); break;
			case "OBJ_PAV": Pavadinimas=val; break;
			case "ADRESAS": Adresas = val; break;
			case "JAD_TEKSTAS": JADTekstas = val; break;
			case "ARO_KODAS": AOB=rdr.ValLong(); break;
			case "ADR_BUS": AdrBus = rdr.ValInt(); break;
			case "OBJ_ID_NTR": NTRId = rdr.ValLong(); break;
			case "NTR_OBJ_KODAS": NTRKodas = val; break;
			case "NJA_ID": NjaId = rdr.ValInt(); break;
			case "STE_ID": SteId=rdr.ValInt(); break;
			default: Console.WriteLine($"JAObjAsm: {key}: {val}"); break;
		}
	}
}
public class JaFinAts : ILoadXML {
	public DateOnly? DataNuo { get; set; }
	public DateOnly? DataIki { get; set; }
	public DateOnly? DataPateik { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch(key){
			case "FIN_LAIKOTARPIS_IKI": DataIki=rdr.ValDate(); break;
			case "FIN_LAIKOTARPIS_NUO": DataNuo=rdr.ValDate(); break;
			case "FIN_PATEIK_DATA": DataPateik=rdr.ValDate(); break;
			default: Console.WriteLine($"JAFinAts: {key}: {val}"); break;
		}
	}
}
public class JaFilialas : ILoadXML {
	public long Kodas { get; set; }
	public string? Pavadinimas { get; set; }
	public DateOnly? DataReg { get; set; }
	public DateOnly? DataSteig { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch (key) {
			case "FIL_OBJ_KODAS": Kodas = rdr.ValLong()??-1; break;
			case "FIL_OBJ_PAV": Pavadinimas = val; break;
			case "FIL_OBJ_REG_DATA": DataReg = rdr.ValDate(); break;
			case "FIL_OBJ_STEIGIMO_DATA": DataSteig = rdr.ValDate(); break;
			default: Console.WriteLine($"JAFilialas: {key}: {val}"); break;
		}
	}
}
public class JaFaktai : ILoadXML {
	public string? Aprasymas { get; set; }
	public DateOnly? DataPradzios { get; set; }
	public DateOnly? DataPabaigos { get; set; }
	public DateOnly? DataTermNuo { get; set; }
	public DateOnly? DataTermIki { get; set; }
	public int Tipas { get; set; }
	public int Potipis { get; set; }
	public int? Anul { get; set; }
	public int? Kvo { get; set; }
    public int? KatId { get; set; }
	public long? ProkurNr { get; set; }
	public List<JaFaktNaud>? Naudotojai { get; set; }
	public List<JaFaktDok>? Dokumentai { get; set; }
	public List<JaFaktKat>? KatFaktai { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch(key){
			case "FAK_APRASYMAS": Aprasymas=val; break;
			case "FAK_PRADZIOS_DATA": DataPradzios=rdr.ValDate(); break;
			case "FAK_PABAIGOS_DATA": DataPabaigos=rdr.ValDate(); break;
			case "FAK_TERMINAS_NUO": DataTermNuo = rdr.ValDate(); break;
			case "FAK_TERMINO_IKI": DataTermIki = rdr.ValDate(); break;
			case "FAKT_TIPAS": Tipas=rdr.ValInt()??-1; break;
			case "FAKP_POTIPIS": Potipis=rdr.ValInt()??-1; break;
			case "FAK_ANUL": Anul=rdr.ValInt(); break;
			case "FAK_KVO": Kvo=rdr.ValInt(); break;
			case "KAT_ID": KatId=rdr.ValInt(); break;
			case "PROKUROS_NR": ProkurNr=rdr.ValLong(); break;
			default: Console.WriteLine($"JAFaktai: {key}: {val}"); break;
		}
	}
	void ILoadXML.LoadData(string key, XmlReader rdr) {
		switch(key){
			case "FAKTU_NAUDOTOJAI": (Naudotojai??=[]).Add(rdr.Fill<JaFaktNaud>()); break;
			case "FAKTU_DOKUMENTAI": (Dokumentai??=[]).Add(rdr.Fill<JaFaktDok>()); break;
			case "KAT_FAKTAI": (KatFaktai??=[]).Add(rdr.Fill<JaFaktKat>()); break;
			default: Console.WriteLine($"JAFaktai Missing: {key}"); break;
		}
	}
}
public class JaFaktDok : ILoadXML {
	public long ID { get; set; }
	public int? Poz { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch (key) {
			case "DOK_ID": ID = rdr.ValInt() ?? -1; break;
			case "FDO_POZ": case "NDO_POZ": Poz = rdr.ValInt(); break;
			default: Console.WriteLine($"JAFaktDok: {key}: {val}"); break;
		}
	}
}
public class JaFaktKat : ILoadXML {
	public int Tipas { get; set; }
	public int Potipis { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch (key) {
			case "FAKT_TIPAS": Tipas = rdr.ValInt() ?? -1; break;
			case "FAKP_POTIPIS": Potipis = rdr.ValInt() ?? -1; break;
			default: Console.WriteLine($"JaFaktKat: {key}: {val}"); break;
		}
	}
}
public class JaFaktNaud : ILoadXML {
	public long ID { get; set; }
	public int? JungtNuosavybe { get; set; }
	public int? Par { get; set; }
    public string? Aprasymas { get; set; }
	public int? Kapitalas { get; set; }
	public int? KVONarys { get; set; }
	public DateOnly? DataNuo { get; set; }
	public DateOnly? DataIki { get; set; }
	public DateOnly? DataPradzios { get; set; }
	public DateOnly? DataPabaigos { get; set; }
	public List<JaFaktDok>? Dokumentai { get; set; }
    public string? SalKodas { get; set; }
    void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch(key){
			case "OBA_ID": ID=rdr.ValLong()??-1; break;
			case "FNA_JUNGT_NUOSAVYBE": JungtNuosavybe=rdr.ValInt(); break;
			case "PAR_KODAS": Par=rdr.ValInt(); break;
			case "FNA_APRASYMAS": Aprasymas=val; break;
			case "FNA_KAPITALAS": Kapitalas=rdr.ValInt(); break;
			case "FNA_KVO_NARYS": KVONarys=rdr.ValInt(); break;
			case "FNA_DATA_NUO": DataNuo=rdr.ValDate(); break;
			case "FNA_DATA_IKI": DataIki=rdr.ValDate(); break;
			case "FNA_PRADZIOS_DATA": DataPradzios=rdr.ValDate(); break;
			case "FNA_PABAIGOS_DATA": DataPabaigos=rdr.ValDate(); break;				
			case "VALI_TRIRAIDIS_KODAS": SalKodas=val; break;				
			default: Console.WriteLine($"JAFaktNaud: {key}: {val}"); break;
		}
	}
	void ILoadXML.LoadData(string key, XmlReader rdr) {
		switch(key){
			case "NAUDOTOJU_DOKUMENTAI": (Dokumentai??=[]).Add(rdr.Fill<JaFaktDok>()); break;
			default: Console.WriteLine($"JAFakt Missing: {key}"); rdr.Read(); break;
		}
	}
}



public class JaFizAsm : ILoadXML {
	public long ID { get; set; }
	public long Kodas { get; set; }
	public int? Salis { get; set; }
	public string? Vardas { get; set; }
	public string? Pavarde { get; set; }
	public DateOnly? DataGimimo { get; set; }
	public DateOnly? DataMirties { get; set; }
	public string? UzsienKodas { get; set; }
	public int? AdrBus { get; set; }
	public int? Anul { get; set; }
	public List<JaFizAsmAdr>? Adresai { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch(key){
			case "FIZ_ID": ID=rdr.ValLong()??-1; break;
			case "FIZ_KODAS": Kodas=rdr.ValLong()??-1; break;
			case "FIZ_SAL_KODAS": Salis=rdr.ValInt(); break;
			case "FAV_VARDAS": Vardas=val?.FCase(); break;
			case "FAV_PAVARDE": Pavarde = val?.FCase(); break;
			case "FIZ_GIMIMO_DATA": DataGimimo = rdr.ValDate(); break;
			case "FIZ_MIRTIES_DATA": DataMirties = rdr.ValDate(); break;
			case "FIZ_UZSIENIO_KODAS": UzsienKodas = val; break;
			case "ADR_BUS": AdrBus=rdr.ValInt(); break;
			case "FIZ_ANUL": Anul=rdr.ValInt(); break;
			default: Console.WriteLine($"JAFizAsm: {key}: {val}"); break;
		}
	}
	void ILoadXML.LoadData(string key, XmlReader rdr) {
		switch(key){
			case "FIZINIU_ADRESAI": (Adresai??=[]).Add(rdr.Fill<JaFizAsmAdr>()); break;
			default: Console.WriteLine($"JAFakt Missing: {key}"); rdr.Read(); break;
		}
	}
}
public class JaFizAsmAdr : ILoadXML {
	public long? AOB { get; set; }
	public string? Adresas { get; set; }	
	public int? AdrSalis { get; set; }
	public int? AdrBus { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch(key){
			case "ARO_KODAS": AOB=rdr.ValLong()??-1; break;
			case "ASM_ADRESAS": Adresas=val; break;
			case "SAL_KODAS": AdrSalis=rdr.ValInt(); break;
			case "ADR_BUS": AdrBus=rdr.ValInt(); break;
			default: Console.WriteLine($"JAFizAsmAdr: {key}: {val}"); break;
		}
	}
}
public class JaFizNeid : ILoadXML {
	public long ID { get; set; }
	public string? Vardas { get; set; }
	public string? Pavarde { get; set; }
	public string? Adresas { get; set; }
	public string? Pastabos { get; set; }
	public DateOnly? GimimoData { get; set; }
	public DateOnly? MirtiesData { get; set; }
	public long? AOB { get; set; }
	public int? AdrSalis { get; set; }
	public int? AdrBus { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch(key){
			case "NEID_FIZ_ID": ID=rdr.ValLong()??-1; break;
			case "FAV_VARDAS": Vardas = val.FCase(); break;
			case "FAV_PAVARDE": Pavarde = val.FCase(); break;
			case "FIZ_GIMIMO_DATA": GimimoData = rdr.ValDate(); break;
			case "FIZ_MIRTIES_DATA": MirtiesData = rdr.ValDate(); break;
			case "ASM_ADRESAS": Adresas = val; break;
			case "FIZ_PASTABOS": Pastabos = val; break;
			case "ARO_KODAS": AOB = rdr.ValLong() ?? -1; break;
			case "FIZ_SAL_KODAS": AdrSalis=rdr.ValInt(); break;
			case "ADR_BUS": AdrBus=rdr.ValInt(); break;
			default: Console.WriteLine($"JAFizneid: {key}: {val}"); break;
		}
	}
}
public class JaDok : ILoadXML {
	public long ID { get; set; }
	public string? Nr { get; set; }
	public string? Pastabos { get; set; }
	public DateOnly? Data { get; set; }
	public DateOnly? DataPateik { get; set; }
	public DateOnly? DataReg { get; set; }
	public int Tipas { get; set; }
	public int Potipis { get; set; }
	public string? NotaroNr { get; set; }
	public long? Kodas { get; set; }
	public int? FizId { get; set; }
	public int? Anul { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch (key) {
			case "DOK_ID": ID = rdr.ValLong() ?? -1; break;
			case "DOK_NR": Nr = val; break;
			case "DOK_PASTABOS": Pastabos = val; break;
			case "DOK_DATA": Data = rdr.ValDate(); break;
			case "DOK_PATEIKIMO_DATA": DataPateik = rdr.ValDate(); break;
			case "DOK_REG_DATA": DataReg = rdr.ValDate(); break;
			case "DOKT_TIPAS": Tipas = rdr.ValInt() ?? -1; break;
			case "DOKP_POTIPIS": Potipis = rdr.ValInt() ?? -1; break;
			case "DOK_NOTARO_REG_NR": NotaroNr = val; break;
			case "DOKI_KODAS": Kodas = rdr.ValInt() ?? -1; break;
			case "FIZ_ID": Kodas = rdr.ValInt() ?? -1; break;
			case "DOK_ANUL": Anul = rdr.ValInt(); break;
			default: Console.WriteLine($"JADok: {key}: {val}"); break;
		}
	}
}
public class JaUzsSteig : ILoadXML {
	public long ID { get; set; }
	public string? Kodas { get; set; }
	public string? Pavadinimas { get; set; }
	public string? Pastaba { get; set; }
	public string? Veikla { get; set; }
	public DateOnly? DataReg { get; set; }
	public string? TeisForma { get; set; }
	public string? TeisStatusas { get; set; }
	public string? Adresas { get; set; }
	public string? Registras { get; set; }
	public int? Salis { get; set; }
	public string? SalKodas { get; set; }
	public long? Kapitalas { get; set; }
    public string? FMetaiNuo { get; set; }
    public List<JaUzsAdres>? Adresai { get; set; }
	public List<JaUzsPavad>? Pavadinimai { get; set; }
	public List<JaUzsStatusas>? Statusai { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch (key) {
			case "STE_ID": ID = rdr.ValLong() ?? -1; break;
			case "STE_KODAS": Kodas = val; break;
			case "STE_PAVAD": Pavadinimas = val; break;
			case "STE_PASTABA": Pastaba = val; break;
			case "STE_VEIKLA": Veikla = val; break;
			case "STE_TEISINE_FORMA": TeisForma = val; break;
			case "STE_TEISINIS_STATUSAS": TeisStatusas = val; break;
			case "STE_ADRESAS": Adresas = val?.FCase(); break;
			case "STE_REG_DATA": DataReg = rdr.ValDate(); break;
			case "STE_REGISTRAS": Registras = val; break;
			case "SAL_KODAS": Salis = rdr.ValInt(); break;
			case "STE_KAPITALAS": Kapitalas = rdr.ValLong(); break;
			case "STE_FMETAI_NUO": FMetaiNuo = val; break;
			case "VALI_TRIRAIDIS_KODAS": SalKodas = val; break;
			default: Console.WriteLine($"JaUzsSteig: {key}: {val}"); break;
		}
	}
	void ILoadXML.LoadData(string key, XmlReader rdr) {
		switch (key) {
			case "UZSIENIO_PAVADINIMAI": (Pavadinimai ??= []).Add(rdr.Fill<JaUzsPavad>()); break;
			case "UZSIENIO_ADRESAI": (Adresai ??= []).Add(rdr.Fill<JaUzsAdres>()); break;
			case "UZSIENIO_STATUSAI": (Statusai ??= []).Add(rdr.Fill<JaUzsStatusas>()); break;
			default: Console.WriteLine($"JAFakt Missing: {key}"); rdr.Read(); break;
		}
	}
}
public class JaUzsStatusas : ILoadXML {
	public string? Statusas { get; set; }
	public DateOnly? Data { get; set; }
	public DateOnly? DataNuo { get; set; }
	public DateOnly? DataIki { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch (key) {
			case "UST_STATUSAS": Statusas = val; break;
			case "UST_STATUSO_DATA": Data = rdr.ValDate(); break;
			case "UST_DATA_NUO": DataNuo = rdr.ValDate(); break;
			case "UST_DATA_IKI": DataIki = rdr.ValDate(); break;
			default: Console.WriteLine($"JaUzsStatusas: {key}: {val}"); break;
		}
	}
}
public class JaUzsPavad : ILoadXML {
	public string? Pavadinimas { get; set; }
	public DateOnly? DataNuo { get; set; }
	public DateOnly? DataIki { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch (key) {
			case "UPA_PAVAD": Pavadinimas = val; break;
			case "UPA_DATA_NUO": DataNuo = rdr.ValDate(); break;
			case "UPA_DATA_IKI": DataIki = rdr.ValDate(); break;
			default: Console.WriteLine($"JaUzsPavad: {key}: {val}"); break;
		}
	}
}
public class JaUzsAdres : ILoadXML {
	public string? Adresas { get; set; }
	public DateOnly? DataNuo { get; set; }
	public DateOnly? DataIki { get; set; }
	void ILoadXML.LoadAttr(string key, string val, XmlReader rdr) {
		switch (key) {
			case "UAD_ADRESAS": Adresas = val; break;
			case "UAD_DATA_NUO": DataNuo = rdr.ValDate(); break;
			case "UAD_DATA_IKI": DataIki = rdr.ValDate(); break;
			default: Console.WriteLine($"JaUzsAdres: {key}: {val}"); break;
		}
	}
}
