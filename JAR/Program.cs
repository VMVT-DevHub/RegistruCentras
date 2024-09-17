using JAR;
using RC;
using RC.Classes;
using RC.Services;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;
using System.Text;
using System.Text.Json;
using Npgsql;
using NpgsqlTypes;
using Npgsql.Replication.PgOutput.Messages;

// Get config
var cfg = new Configuration().Data;
DB.ConnStr = cfg.ConnectionString;
Console.SetOut(new OutputLog(Console.Out) { Print = cfg.PrintOutput });
var tbl = cfg.Tables ?? [];

// Initialize RC object
var rc = new RegistruCentras(cfg.Cert, cfg.Pass) {
	User = cfg.User, Url = cfg.Url
};
// Initialize JAR object
var jar = rc.GetJAR();
jar.Threads = cfg.Threads;

if (args.Length > 0 && args[0] == "clf") {
	var clf = await jar.KlasifPilnas();
	//if(clf.error...

	if (clf.Data?.Count > 0) {
		var fld = new List<string>(["clf_grupe", "clf_kodas", "clf_tipas", "clf_potipis", "clf_reiksme", "clf_pavadinimas", "clf_pavadinimas_en", "clf_data_nuo", "clf_data_iki"]);
		var blk = new DBBulk("temp_rc_clf", fld, cfg.ConnectionString);
		using (var tmp = new NpgsqlCommand("CREATE TEMPORARY TABLE temp_rc_clf (clf_grupe varchar(255), clf_kodas integer, clf_tipas integer, clf_potipis integer, clf_reiksme varchar(255), clf_pavadinimas varchar, clf_pavadinimas_en varchar, clf_data_nuo timestamp(0), clf_data_iki timestamp(0))", blk.Conn))
			await tmp.ExecuteNonQueryAsync();
		foreach (var i in clf.Data) {
			Console.WriteLine(i.Kodas);
			foreach (var j in i.Atributai) {
				blk.Insert([i.Kodas, j.Kodas, j.Tipas, j.Potipis, j.Reiksme, j.Pavadinimas, j.PavadinimasEn, j.Data, j.GaliojaIki]);
			}
		}
		await blk.CompleteAsync();
		using (var trn = new NpgsqlCommand("TRUNCATE TABLE jar.raw_rc_klasifikatoriai; " +
			$"INSERT INTO jar.raw_rc_klasifikatoriai ({string.Join(",",fld)}) SELECT {string.Join(",",fld)} FROM temp_rc_clf;", blk.Conn))
			await trn.ExecuteNonQueryAsync();

		await blk.Conn.CloseAsync(); await blk.Conn.DisposeAsync();
	}
}
else {
	List<long> lst;
	var loop = 0;
	do {
		// Get items to update
		using var rdr = await DB.Read($"{cfg.ItemQuery} LIMIT {cfg.ItemLimit}");
		lst = []; while (rdr.Read()) lst.Add(rdr.GetInt32(0));

		if (lst.Count > 0) {
			// Initialize binary data connections
			foreach (var i in tbl) await i.Value.Init(cfg);

			loop++;
			var cnt = 1;
			await jar.Info(lst,
				async (id, dt) => {
					var dtt = dt.Data;
					var wrt = $"{cnt++}\t{id}: ";

					if (dt.Status != 1) {
						wrt = $"Error - {dt.Code}: {dt.Message}";
						await DB.Execute("select 1;");
					}
					else if (dtt?.Objektai is not null) {
						wrt += dtt.Pavadinimas?.Chop(75);
						var obj = dtt.Objektai;
						tbl["jarobj"].Blk?.Insert([obj.ID, obj.Kodas, obj.Pavadinimas, obj.Pastabos, obj.Komentaras, obj.PristDezAdr, obj.Forma, obj.NTRKodas, obj.DBUKKodas, obj.OTIPKodas,
				obj.REJEKodas, obj.DataReg, obj.DataIsreg, obj.DataSteigimo, obj.Anul, obj.Tikslai, obj.Pagrindinis, obj.Versija, obj.VersData, obj.PagrID, obj.PagrKodas, obj.PagrPavadinimas]);

						;
						if (obj.Formos?.Count > 0) foreach (var j in obj.Formos) if (j.Kodas > 0) tbl["jaform"].Blk?.Insert([obj.ID, j.Kodas, j.DataNuo, j.DataIki, j.Anul]);
						if (obj.Atributai?.Count > 0) foreach (var j in obj.Atributai) if (j.Kodas > 0) tbl["jaatri"].Blk?.Insert([obj.ID, j.Kodas, j.Reiksme, j.DataNuo, j.DataIki, j.VienKodas]);
						if (obj.Statusai?.Count > 0) foreach (var j in obj.Statusai) if (j.Statusas > 0) tbl["jastat"].Blk?.Insert([obj.ID, j.Statusas, j.DataIgijimo, j.DataNetekimo, j.DataNuo, j.DataIki, j.IsPrKodas, j.Anul]);
						if (obj.Veiklos?.Count > 0) foreach (var j in obj.Veiklos) if (j.Kodas > 0) tbl["javeik"].Blk?.Insert([obj.ID, j.Kodas, j.Versija, j.DataNuo, j.DataIki]);
						if (obj.Pavadinimai?.Count > 0) foreach (var j in obj.Pavadinimai) if (j.Kodas > 0) tbl["japavd"].Blk?.Insert([obj.ID, j.Kodas, j.RegNr, j.DataReg, j.DataNuo, j.DataIki, j.Pavadinimas, j.Anul]);
						if (obj.Adresai?.Count > 0) foreach (var j in obj.Adresai) tbl["jaadre"].Blk?.Insert([obj.ID, j.AOB, j.Pavadinimas, j.DataNuo, j.DataIki, j.AdrBus]);
						if (obj.TekstiniaiDuomenys?.Count > 0) foreach (var j in obj.TekstiniaiDuomenys) if (j.Kodas > 0) tbl["jatext"].Blk?.Insert([obj.ID, j.Kodas, j.Tekstas, j.DataNuo, j.DataIki]);
						if (obj.NTObjektai?.Count > 0) foreach (var j in obj.NTObjektai) if (j.NTR > 0) tbl["jantob"].Blk?.Insert([obj.ID, j.NTR, j.DataNuo, j.DataIki]);
						if (obj.ObjektuAsmenys?.Count > 0) foreach (var j in obj.ObjektuAsmenys) if (j.ID > 0) tbl["jaasmn"].Blk?.Insert([obj.ID, j.ID, j.FaId, j.JaId, j.NeidId, j.NjaId, j.SteId, j.NTRId, j.NTRKodas]);
						if (obj.FinansineAtskaitomybe?.Count > 0) foreach (var j in obj.FinansineAtskaitomybe) tbl["jafina"].Blk?.Insert([obj.ID, j.DataNuo, j.DataIki, j.DataPateik]);
						if (obj.Filialai?.Count > 0) foreach (var j in obj.Filialai) if (j.Kodas > 0) tbl["jafili"].Blk?.Insert([obj.ID, j.Kodas, j.Pavadinimas, j.DataReg, j.DataSteig]);

						if (obj.Faktai?.Count > 0) foreach (var j in obj.Faktai) {
								List<long>? dok = null; if (j.Dokumentai is not null) { dok = []; foreach (var k in j.Dokumentai) dok.Add(k.ID); }
								tbl["jafakt"].Blk?.Insert([obj.ID, j.Aprasymas, j.DataPradzios, j.DataPabaigos, j.DataTermNuo, j.DataTermIki, j.Tipas, j.Potipis, j.Anul, j.Kvo, j.KatId, j.ProkurNr, dok, JsonSerializer.Serialize(j.Naudotojai), JsonSerializer.Serialize(j.KatFaktai)],
									[null, null, null, null, null, null, null, null, null, null, null, null, null, NpgsqlDbType.Jsonb, NpgsqlDbType.Jsonb]);
							}

						if (dtt.Dokumentai?.Count > 0) foreach (var j in dtt.Dokumentai) if (j.ID > 0) tbl["dokume"].Blk?.Insert([j.ID, j.Kodas, j.Nr, j.Pastabos, j.Data, j.DataPateik, j.DataReg, j.Tipas, j.Potipis, j.NotaroNr, j.FizId, j.Anul]);
						if (dtt.FiziniaiNeid?.Count > 0) foreach (var j in dtt.FiziniaiNeid) if (j.ID > 0) tbl["fizned"].Blk?.Insert([j.ID, j.Vardas, j.Pavarde, j.Pastabos, j.GimimoData, j.MirtiesData, j.AOB, j.Adresas, j.AdrSalis, j.AdrBus]);
						if (dtt.FiziniaiAsmenys?.Count > 0) foreach (var j in dtt.FiziniaiAsmenys) if (j.ID > 0) {
									tbl["fizasm"].Blk?.Insert([j.ID, j.Kodas, j.Salis, j.Vardas, j.Pavarde, j.DataGimimo, j.DataMirties, j.UzsienKodas, j.AdrBus, j.Anul]);
									if (j.Adresai?.Count > 0) foreach (var k in j.Adresai) tbl["fizadr"].Blk?.Insert([j.ID, k.AOB, k.Adresas, k.AdrSalis, k.AdrBus]);
								}

						if (dtt.UzsienioSteigejai?.Count > 0) foreach (var j in dtt.UzsienioSteigejai) if (j.ID > 0) {
									tbl["uzstei"].Blk?.Insert([j.ID, j.Kodas, j.Pavadinimas, j.Pastaba, j.Veikla, j.DataReg, j.TeisForma, j.TeisStatusas, j.Adresas, j.Registras, j.Salis, j.SalKodas, j.Kapitalas, j.FMetaiNuo]);
									if (j.Statusai?.Count > 0) foreach (var k in j.Statusai) tbl["uzstat"].Blk?.Insert([j.ID, k.Statusas, k.Data, k.DataNuo, k.DataIki]);
									if (j.Pavadinimai?.Count > 0) foreach (var k in j.Pavadinimai) tbl["uzpava"].Blk?.Insert([j.ID, k.Pavadinimas, k.DataNuo, k.DataIki]);
									if (j.Adresai?.Count > 0) foreach (var k in j.Adresai) tbl["uzadre"].Blk?.Insert([j.ID, k.Adresas, k.DataNuo, k.DataIki]);
								}
					}
					Console.WriteLine(wrt);
				}, RC.Services.JAR.ActionType.Isplestinis);


			// Commit changes
			var drp = new List<string>();
			foreach (var i in tbl) {
				var j = i.Value;
				await j.Complete();
				if (j.Blk is not null) {
					if (i.Key == cfg.MainTable) {
						using var log = new NpgsqlCommand("INSERT INTO jar.log_updates (log_jar) SELECT jar_kodas FROM temp_raw_rc_jar;", j.Blk.Conn);
						Console.WriteLine($"\nUpdates: {await log.ExecuteNonQueryAsync()}");
					}
					await j.Blk.Conn.CloseAsync(); await j.Blk.Conn.DisposeAsync();
				}
			}
		} else { Console.WriteLine($"\nUpdates: 0"); }
		Console.Write("\n");
	} while (lst.Count >= cfg.ItemLimit * 0.95 && loop < cfg.Loops);

	await DB.Execute(cfg.ItemComplete);

}
Console.WriteLine("Done.");

//var dt1 = await jar.Klasifikatoriai();
//var dt2 = await jar.Atributai("SALYS");
//var dt3 = await jar.KlasifPilnas();
