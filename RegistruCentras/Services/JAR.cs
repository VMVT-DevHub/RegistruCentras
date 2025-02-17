using RC.Classes;
using System.Threading;

namespace RC.Services;


public class JARObjektas : RCResponse { public JaResponse? Data { get; set; } public JARObjektas(RCDataResponse rsp){ var r=rsp.Response; Code=r.Code; Status=r.Status; Message=r.Message; Data=rsp.JA; } }
public class JARKlasif : RCResponse { public List<RowKlasif>? Data { get; set; } public JARKlasif(RCDataResponse rsp){ var r=rsp.Response; Code=r.Code; Status=r.Status; Message=r.Message; Data=rsp.GetKlasifikatoriai(); } }
public class JARAtributai : RCResponse { public List<RowAtributas>? Data { get; set; } public JARAtributai(RCDataResponse rsp){ var r=rsp.Response; Code=r.Code; Status=r.Status; Message=r.Message; Data=rsp.GetAtributai(); } }


public  class JAR(RegistruCentras rc) {
	private RegistruCentras RC { get; set; } = rc;
	public int Threads { get; set; } = 5;

	public async Task<JARObjektas> Trumpas(long ja) => await Info(ja, ActionType.Trumpas);
	public async Task<JARObjektas> Pagrindinis(long ja) => await Info(ja, ActionType.Pagrindinis);
	public async Task<JARObjektas> Isplestinis(long ja) => await Info(ja, ActionType.Isplestinis);
	public async Task<JARObjektas> SuIstorija(long ja) => await Info(ja, ActionType.SuIstorija);
	public async Task<JARObjektas> Info(long ja, ActionType at) {
		var dt = await RC.Sign(new RCDataRequest(){ ID=ja, ActionType = (int)at, CallerCode = RC.User }).Execute(RC.Url);
		return new JARObjektas(dt);
	}

	public async Task<List<JARObjektas>> Info(List<long> ja, ActionType at) {
		var ret = new List<JARObjektas>();
		var tsks = new List<Task>();
		var smf = new SemaphoreSlim(Threads);
		foreach (var i in ja) {
			await smf.WaitAsync();
			tsks.Add(Task.Run(async () => { try { ret.Add(await Info(i, at)); } finally { smf.Release(); } }));
		}
		await Task.WhenAll([.. tsks]);
		return ret;
	}
	public async Task Info(List<long> ja, Func<long,JARObjektas,Task> act, ActionType at) {
		var tsks = new List<Task>();
		var smf = new SemaphoreSlim(Threads);
		foreach (var i in ja) {
			await smf.WaitAsync();
			tsks.Add(Task.Run(async () => { try { await act(i, await Info(i, at)); } catch (Exception ex) {
					throw new (ex.Message);
				} finally { smf.Release(); } }));
		}
		await Task.WhenAll([.. tsks]);
	}

	public async Task<JARAtributai> Atributai(string pavad){
		var dt = await RC.Sign(new RCDataRequest(){ Klasifikatorius=pavad, ActionType = (int)ActionType.Klasifikatoriai, CallerCode = RC.User }).Execute(RC.Url);
		return new JARAtributai(dt);
	}
	public async Task<JARKlasif> Klasifikatoriai(){
		var dt = await RC.Sign(new RCDataRequest(){ ActionType = (int)ActionType.Klasifikatoriai, CallerCode = RC.User }).Execute(RC.Url);
		return new JARKlasif(dt);
	}

	public async Task<JARKlasif> KlasifPilnas(){
		var cl = await Klasifikatoriai();
		if(cl.Status==1 && cl.Data is not null){
			var tsks = new List<Task>();
			foreach(var i in cl.Data){
				tsks.Add(Task.Run(async () => {
					var dt = await RC.Sign(new RCDataRequest(){ Klasifikatorius=i.Kodas, ActionType = (int)ActionType.Klasifikatoriai, CallerCode = RC.User }).Execute(RC.Url);
					i.Atributai=dt.GetAtributai();
				}));
			}
			Task.WaitAll([.. tsks]);
		}
		return cl;
	}


	public async Task<JARAtributai> ListNauji(DateTime date) {
		var dt = await RC.Sign(new RCDataRequest() { Data=date, ActionType = (int)ActionType.ListNauji, CallerCode = RC.User }).Execute(RC.Url);
		return new JARAtributai(dt);
	}
	public async Task<JARAtributai> ListIsreg(DateTime date) {
		var dt = await RC.Sign(new RCDataRequest() { Data = date, ActionType = (int)ActionType.ListIsreg, CallerCode = RC.User }).Execute(RC.Url);
		return new JARAtributai(dt);
	}
	public async Task<JARAtributai> ListKeisti(DateTime date) {
		var dt = await RC.Sign(new RCDataRequest() { Data = date, ActionType = (int)ActionType.ListKeisti, CallerCode = RC.User }).Execute(RC.Url);
		return new JARAtributai(dt);
	}

	public enum ActionType {
		Trumpas = 76,
		Pagrindinis= 77,
		Isplestinis = 17,
		SuIstorija = 49,
		Klasifikatoriai = 701,
		ListNauji = 46,
		ListKeisti = 47,
		ListIsreg = 48
	}
}
