using System.Xml.Serialization;

namespace RC.Classes;

[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")] 
public class SoapEnvelope<T> where T : SoapBody {
	[XmlElement("Header")] public SoapHeader? Header { get; set; } 
	[XmlElement("Body")] public T? Body { get; set; } 
}
public class SoapHeader { }
public class SoapFault {
	[XmlIgnore] public bool Error { get; set; } = true;
	[XmlElement("faultcode", Namespace = "")] public string? Code { get; set; }
	[XmlElement("faultstring", Namespace = "")] public string? Message { get; set; }
}
public class SoapBody {	[XmlElement("Fault")] public SoapFault? Fault { get; set; } }


public class SoapRequest : SoapBody {
	[XmlElement("GetData", Namespace = "urn:http://www.registrucentras.lt")] public SoapRequestData? Request { get; set; }
}

public class SoapResponse : SoapBody {
	[XmlElement("GetDataResponse", Namespace = "urn:http://www.registrucentras.lt")] public RCDataResponse? Response { get; set; }
}


public class SoapRequestData {
	[XmlAttribute("encodingStyle", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")] public string EncodingStyle { get; set; } = "http://schemas.xmlsoap.org/soap/encoding/";
	[XmlElement("input", Namespace = "http://www.registrucentras.lt")] public RCDataRequest? Input { get; set; }
}