using JAR;
using RC;
using RC.Classes;
using RC.Services;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;
using System.Text;
using System.Text.Json;


var cfg = new Configuration().Data;
DB.ConnStr = cfg.ConnectionString;

var rc = new RegistruCentras(cfg.Cert, cfg.Pass) {
	User = cfg.User, Url = cfg.Url
};

var jar = rc.GetJAR();
jar.Threads = 20;

TextWriter customOutput = new OutputLog(Console.Out);
Console.SetOut(customOutput);


Func<string, string> chop = (str) => {
	var lmt = 75; str = new string(str.Take(lmt).ToArray())??"";
	char[] chr = new char[lmt];
	str.CopyTo(0, chr, 0, str.Length);
	for (int i = str.Length; i < str.Length; i++) chr[i] = ' ';
	return new string(chr);
};


var trn = new List<string>(); foreach(var i in cfg.Tables ?? []) trn.Add($"TRUNCATE TABLE {i.Value.Table};");
await DB.Execute(string.Join("",trn));

using var rc_dokume = cfg.Get("dokume"); using var rc_fizasm = cfg.Get("fizasm"); using var rc_fizadr = cfg.Get("fizadr"); 
using var rc_fizned = cfg.Get("fizned"); using var rc_uzstei = cfg.Get("uzstei"); using var rc_uzstat = cfg.Get("uzstat");
using var rc_uzpava = cfg.Get("uzpava"); using var rc_uzadre = cfg.Get("uzadre"); using var rc_jarobj = cfg.Get("jarobj");
using var rc_jaform = cfg.Get("jaform"); using var rc_jaatri = cfg.Get("jaatri"); using var rc_jastat = cfg.Get("jastat");
using var rc_javeik = cfg.Get("javeik"); using var rc_japavd = cfg.Get("japavd"); using var rc_jaadre = cfg.Get("jaadre");
using var rc_jatext = cfg.Get("jatext"); using var rc_jantob = cfg.Get("jantob"); using var rc_jaasmn = cfg.Get("jaasmn");
using var rc_jafina = cfg.Get("jafina"); using var rc_jafili = cfg.Get("jafili"); using var rc_jafakt = cfg.Get("jafakt");

var cnt = 1;
await jar.Info([1245, 67890], async (id, dt) => {
	var dtt = dt.Data;
	var wrt = $"{cnt++}\t{id}: ";

	if (dt.Status != 1) {
		wrt = $"Error - {dt.Code}: {dt.Message}";
		await DB.Execute("select 1;");
	}
	else if (dtt?.Objektai is not null) {
		wrt += chop(dtt.Pavadinimas ?? "");
		var obj = dtt.Objektai;
		rc_jarobj?.Insert([obj.ID, obj.Kodas, obj.Pavadinimas, obj.Pastabos, obj.Komentaras, obj.PristDezAdr, obj.Forma, obj.NTObjektai, obj.DBUKKodas, obj.OTIPKodas, obj.REJEKodas, obj.DataReg, obj.DataIsreg, obj.DataSteigimo, obj.Anul, obj.Tikslai, obj.Pagrindinis, obj.Versija, obj.VersData, obj.PagrID, obj.PagrKodas, obj.PagrPavadinimas]);

		if (obj.Formos?.Count > 0) foreach (var j in obj.Formos) if (j.Kodas > 0) rc_jaform?.Insert([obj.ID, j.Kodas, j.DataNuo, j.DataIki, j.Anul]);
		if (obj.Atributai?.Count > 0) foreach (var j in obj.Atributai) if (j.Kodas > 0) rc_jaatri?.Insert([obj.ID, j.Kodas, j.Reiksme, j.DataNuo, j.DataIki, j.VienKodas]);
		if (obj.Statusai?.Count > 0) foreach (var j in obj.Statusai) if (j.Statusas > 0) rc_jastat?.Insert([obj.ID, j.Statusas, j.DataIgijimo, j.DataNetekimo, j.DataNuo, j.DataIki, j.IsPrKodas, j.Anul]);
		if (obj.Veiklos?.Count > 0) foreach (var j in obj.Veiklos) if (j.Kodas > 0) rc_javeik?.Insert([obj.ID, j.Kodas, j.Versija, j.DataNuo, j.DataIki]);
		if (obj.Pavadinimai?.Count > 0) foreach (var j in obj.Pavadinimai) if (j.Kodas > 0) rc_japavd?.Insert([obj.ID, j.Kodas, j.RegNr, j.DataReg, j.DataNuo, j.DataIki, j.Pavadinimas, j.Anul]);
		if (obj.Adresai?.Count > 0) foreach (var j in obj.Adresai) rc_jaadre?.Insert([obj.ID, j.AOB, j.Pavadinimas, j.DataNuo, j.DataIki, j.AdrBus]);
		if (obj.TekstiniaiDuomenys?.Count > 0) foreach (var j in obj.TekstiniaiDuomenys) if (j.Kodas > 0) rc_jatext?.Insert([obj.ID, j.Kodas, j.DataNuo, j.DataIki, j.Tekstas]);
		if (obj.NTObjektai?.Count > 0) foreach (var j in obj.NTObjektai) if (j.NTR > 0) rc_jantob?.Insert([obj.ID, j.NTR, j.DataNuo, j.DataIki]);
		if (obj.ObjektuAsmenys?.Count > 0) foreach (var j in obj.ObjektuAsmenys) if (j.ID > 0) rc_jaasmn?.Insert([obj.ID, j.ID, j.FaId, j.JaId, j.NeidId, j.NjaId, j.SteId]);
		if (obj.FinansineAtskaitomybe?.Count > 0) foreach (var j in obj.FinansineAtskaitomybe) rc_jafina?.Insert([obj.ID, j.DataNuo, j.DataIki, j.DataPateik]);
		if (obj.Filialai?.Count > 0) foreach (var j in obj.Filialai) if (j.Kodas > 0) rc_jafili?.Insert([obj.ID, j.Kodas, j.Pavadinimas, j.DataReg, j.DataSteig]);

		if (obj.Faktai?.Count > 0) foreach (var j in obj.Faktai) {
				List<long>? dok = null; List<byte[]>? nau = null; List<byte[]>? kat = null;
				if (j.Dokumentai is not null) { dok = []; foreach (var k in j.Dokumentai) dok.Add(k.ID); }
				if (j.Naudotojai is not null) { nau = []; foreach (var k in j.Naudotojai) nau.Add(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(k))); }
				if (j.KatFaktai is not null) { kat = []; foreach (var k in j.KatFaktai) kat.Add(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(k))); }

				rc_jafakt?.Insert([obj.ID, j.Aprasymas, j.DataPradzios, j.DataPabaigos, j.DataTermNuo, j.DataTermIki, j.Tipas, j.Potipis, j.Anul, j.Kvo, j.KatId, j.ProkurNr, dok, nau, kat]);
			}

		if (dtt.Dokumentai?.Count > 0) foreach (var j in dtt.Dokumentai) if (j.ID > 0) rc_dokume?.Insert([j.ID, j.Kodas, j.Nr, j.Pastabos, j.Data, j.DataPateik, j.DataReg, j.Tipas, j.Potipis, j.NotaroNr, j.Anul]);
		if (dtt.FiziniaiNeid?.Count > 0) foreach (var j in dtt.FiziniaiNeid) if (j.ID > 0) rc_fizned?.Insert([j.ID, j.Vardas, j.Pavarde, j.Pastabos, j.GimimoData, j.MirtiesData, j.AOB, j.Adresas, j.AdrSalis, j.AdrBus]);
		if (dtt.FiziniaiAsmenys?.Count > 0) foreach (var j in dtt.FiziniaiAsmenys) if (j.ID > 0) {
					rc_fizasm?.Insert([j.ID, j.Kodas, j.Salis, j.Vardas, j.Pavarde, j.DataGimimo, j.DataMirties, j.UzsienKodas, j.AdrBus, j.Anul]);
					if (j.Adresai?.Count > 0) foreach (var k in j.Adresai) rc_fizadr?.Insert([j.ID, k.AOB, k.Adresas, k.AdrSalis, k.AdrBus]);
				}

		if (dtt.UzsienioSteigejai?.Count > 0) foreach (var j in dtt.UzsienioSteigejai) if (j.ID > 0) {
					rc_fizned?.Insert([j.ID, j.Kodas, j.Pavadinimas, j.Pastaba, j.Veikla, j.DataReg, j.TeisForma, j.TeisStatusas, j.Adresas, j.Registras, j.Salis, j.SalKodas, j.Kapitalas, j.FMetaiNuo]);
					if (j.Statusai?.Count > 0) foreach (var k in j.Statusai) rc_uzstat?.Insert([j.ID, k.Statusas, k.Data, k.DataNuo, k.DataIki]);
					if (j.Pavadinimai?.Count > 0) foreach (var k in j.Pavadinimai) rc_uzpava?.Insert([j.ID, k.Pavadinimas, k.DataNuo, k.DataIki]);
					if (j.Adresai?.Count > 0) foreach (var k in j.Adresai) rc_uzadre?.Insert([j.ID, k.Adresas, k.DataNuo, k.DataIki]);
				}
	}
	Console.WriteLine(wrt);
}, RC.Services.JAR.ActionType.SuIstorija);

rc_dokume?.Complete();
rc_fizasm?.Complete();
rc_fizadr?.Complete();
rc_fizned?.Complete();
rc_uzstei?.Complete();
rc_uzstat?.Complete();
rc_uzpava?.Complete();
rc_uzadre?.Complete();
rc_jarobj?.Complete();
rc_jaform?.Complete();
rc_jaatri?.Complete();
rc_jastat?.Complete();
rc_javeik?.Complete();
rc_japavd?.Complete();
rc_jaadre?.Complete();
rc_jatext?.Complete();
rc_jantob?.Complete();
rc_jaasmn?.Complete();
rc_jafina?.Complete();
rc_jafili?.Complete();
rc_jafakt?.Complete();


//var dt1 = await jar.Klasifikatoriai();
//var dt2 = await jar.Atributai("SALYS");
//var dt3 = await jar.KlasifPilnas();


Console.WriteLine("");
