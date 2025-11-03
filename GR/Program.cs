


using GR;
using RC;
using RC.Classes;
using System.Text.Json;


// Get config
var cfg = new Configuration().Data;
Console.SetOut(new OutputLog(Console.Out) { Print = cfg.PrintOutput });
var tbl = cfg.Tables ?? [];

// Initialize RC object
var rc = new RegistruCentras(cfg.Cert, cfg.Pass) {
	User = cfg.User, Url = cfg.Url
};



foreach (string line in File.ReadLines("list.txt")) {
	// Process each line here
	if (long.TryParse(line, out var ak)) {
		Console.Write($"\n{line}|");
		var dt5 = await rc.Sign(new RCDataRequest() { AK = ak, ActionType = 605, CallerCode = rc.User }).Execute(rc.Url);
		if (dt5.GR is not null) Console.Write($"{dt5.GR.AK}|{dt5.GR.MirtiesData}");
		else Console.Write($"Error {dt5.Response.Message}");
	}
	else {
		Console.WriteLine($"Error: {line}");
	}
}


//while (true) {
//	Console.Write("AK: ");
//	var tx = Console.ReadLine();
//	if (!string.IsNullOrEmpty(tx)) {
//		if (long.TryParse(tx, out var ak)) {
//			//var dt1 = await rc.Sign(new RCDataRequest() { AK = ak, ActionType = 588, CallerCode = rc.User }).Execute(rc.Url);
//			//Console.WriteLine("588: " + JsonSerializer.Serialize(dt1.GR));
//			//var dt2 = await rc.Sign(new RCDataRequest() { AK = ak, ActionType = 589, CallerCode = rc.User }).Execute(rc.Url);
//			//Console.WriteLine("589: " + JsonSerializer.Serialize(dt2.GR));
//			//var dt3 = await rc.Sign(new RCDataRequest() { AK = ak, ActionType = 590, CallerCode = rc.User }).Execute(rc.Url);
//			//Console.WriteLine("590: " + JsonSerializer.Serialize(dt3.GR));
//			//var dt4 = await rc.Sign(new RCDataRequest() { AK = ak, ActionType = 591, CallerCode = rc.User }).Execute(rc.Url);
//			//Console.WriteLine("591: " + JsonSerializer.Serialize(dt4.GR));

//			var dt5 = await rc.Sign(new RCDataRequest() { AK = ak, ActionType = 605, CallerCode = rc.User }).Execute(rc.Url);

//			//Console.WriteLine("605: " + JsonSerializer.Serialize(dt5.GR));
//		}
//		else Console.WriteLine("Netinkamas AK");
//	}
//	else break;
//}




