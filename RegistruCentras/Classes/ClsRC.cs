using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using RC.Extensions;


namespace RC.Classes;

public class RCResponse {
	public int Status { get; set; }
	public string Code { get; set; } = "Ok";
	public string? Message { get; set; }

}

public class RCDataResponse : IXmlSerializable {
	public JaResponse? JA { get; set; }
	public GrResponse? GR { get; set; }
	public List<RowSet>? Data { get; set; }
	public RCResponse Response { get; set; } = new();


	public List<RowKlasif>? GetKlasifikatoriai(){ if(Data?.Count>0) {  var ret = new List<RowKlasif>(); foreach (var i in Data) if(i.Klasifikatorius is not null) ret.Add(i.Klasifikatorius); return ret; } return null; }
	public List<RowAtributas>? GetAtributai(){ if(Data?.Count>0) {  var ret = new List<RowAtributas>(); foreach (var i in Data) if(i.Atributas is not null) ret.Add(i.Atributas); return ret; } return null; }


	public XmlSchema? GetSchema() { throw new NotImplementedException(); }
	public void WriteXml(XmlWriter writer) { throw new NotImplementedException(); }
	public void ReadXml(XmlReader reader) {
		reader.ReadToFollowing("return");
		while (reader.Read()) {
			if(reader.IsStartElement()){
				switch (reader.LocalName){
					case "ResponseCode": Response.Status=reader.Next().ValInt()??0;  break;
					case "ResponseData": 
						if(Response.Status==1){
							var dtr  = XmlReader.Create(new Base64Stream(reader));
							while(dtr.Read()) {
								if(dtr.IsStartElement()){
									switch (dtr.LocalName) {
										//TODO: sukurti metodą parsinimui
										case "JAR": JA=dtr.Fill<JaResponse>(); break;
										case "OBJEKTAI": JA=new(dtr.Fill<JaItem>()); break;
										case "ROWSET": Data=[]; break;
										case "ROW": (Data??=[]).Add(dtr.Fill<RowSet>()); break;
										case "personDetailInformation": 
										case "personDetailInformation1": 
										case "personDetailInformation2": 
										case "personDetailInformation3": 
										case "personDetailInformation4": GR = dtr.Fill<GrResponse>(); break;
										default: Console.WriteLine($"Base64 Missing: {dtr.LocalName}"); break;
									}
									if(dtr.NodeType == XmlNodeType.Text) Console.WriteLine($"{new string('\t', dtr.Depth)}->{dtr.Value}");
								}
							}
						} else Response.Message = reader.IsEmptyElement?"Unknown error": reader.Next().Value;
					break; 
					default: Console.WriteLine($"GetDataResponse Missing: {reader.LocalName}"); break; 
				}
			}
		}
	}	
}




public class RCDataRequest : IXmlSerializable {
	public long ID { get; set; }
	public long AK { get; set; }
	public DateTime? Data { get; set; }
	public string? Klasifikatorius { get; set; }
	public int ActionType { get; set; }
	public string? CallerCode { get; set; }
	public string? Parameters { get; set; }
	public string? Time { get; set; }
	public string? Signature { get; set; }

	public byte[] GetBytes(){
		Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssff");
		Parameters = GetParams();
		return Encoding.UTF8.GetBytes($"{ActionType}{CallerCode}{Parameters}{Time}");
	}
	public string GetParams(){
		using var sb = new Utf8StringWriter();
		using (var wrt = XmlWriter.Create(sb)){
			wrt.WriteStartElement("args");
			if(ID>0) wrt.WriteElementString("obj_kodas",ID.ToString());
			//TODO: Sukurti extensions
			if(AK>0) wrt.WriteElementString("asm_kodas", AK.ToString());
			if (Data is not null) wrt.WriteElementString("data", Data.Value.ToString("yyyy-MM-dd"));
			if(Klasifikatorius is not null) wrt.WriteElementString("kla_kodas",Klasifikatorius);
			wrt.WriteElementString("fmt","xml");
			wrt.WriteEndElement();
		}
		return Parameters=sb.ToString();
	}


	private static readonly XmlSerializerNamespaces NS = new ([new XmlQualifiedName("reg", "urn:http://www.registrucentras.lt"), new("soapenv","http://schemas.xmlsoap.org/soap/envelope/")]);
	public string Serialize() {
		using var writer = new Utf8StringWriter();
		new XmlSerializer(typeof(SoapEnvelope<SoapRequest>)).Serialize(writer, new SoapEnvelope<SoapRequest>(){ Header=new(), Body=new(){ Request=new(){ Input=this }}}, NS);
		return writer.ToString();
	}

	public void ReadXml(XmlReader reader) { throw new NotImplementedException(); }
	public void WriteXml(XmlWriter writer) {
		writer.WriteAttributeString("xsi","type",null,"reg:InputParams");
		writer.WriteAttributeString("xmlns","reg",null,"http://www.registrucentras.lt");
		writer.WriteElement("ActionType",ActionType.ToString());
		writer.WriteElement("CallerCode",CallerCode);
		writer.WriteElement("Parameters",Parameters,true);
		writer.WriteElement("Time",Time);
		writer.WriteElement("Signature",Signature);
	}

	XmlSchema? IXmlSerializable.GetSchema() { throw new NotImplementedException(); }

	private static readonly HttpClient HClient = new();
	
	public async Task<RCDataResponse> Execute(string? serviceUrl){
		var req = await HClient.PostAsync(serviceUrl, new StringContent(Serialize(), Encoding.UTF8, "text/xml"));
		using var stream = await req.Content.ReadAsStreamAsync();
		using var reader = XmlReader.Create(stream);
		var rspd = new XmlSerializer(typeof(SoapEnvelope<SoapResponse>)).Deserialize(reader);
		if(rspd is SoapEnvelope<SoapResponse> rsps) {
			if(rsps.Body?.Response is not null) return rsps.Body.Response;
			return new(){ Response=new(){ Status=-1, Code=rsps.Body?.Fault?.Code ?? $"{req.StatusCode} ({(int)req.StatusCode})", Message=rsps.Body?.Fault?.Message ?? "Unknown error" } };
		}
		return new(){ Response=new(){ Status=-1, Code=$"{req.StatusCode} ({(int)req.StatusCode})", Message="Unknown error"} };
	}
	
}








