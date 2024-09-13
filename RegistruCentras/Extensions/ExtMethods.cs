using System.Xml;

namespace RC.Extensions;


public static class Extensions {
	public static int? ValInt(this XmlReader rdr){ if(int.TryParse(rdr.Value,out var val)) return val; else Console.WriteLine($"Wrong Type: {rdr.Name}: (int){rdr.Value}"); return null;}
	public static long? ValLong(this XmlReader rdr){ if(long.TryParse(rdr.Value,out var val)) return val; else Console.WriteLine($"Wrong Type: {rdr.Name}: (long){rdr.Value}"); return null;}
	public static DateOnly? ValDate(this XmlReader rdr){ if(DateOnly.TryParse(rdr.Value,out var val)) return val; else Console.WriteLine($"Wrong Type: {rdr.Name}: (DateOnly){rdr.Value}"); return null;}
	public static DateTime? ValDateTime(this XmlReader rdr){ if(DateTime.TryParse(rdr.Value,out var val)) return val; else Console.WriteLine($"Wrong Type: {rdr.Name}: (DateTime){rdr.Value}"); return null;}
	public static XmlReader Next(this XmlReader rdr) { rdr.Read(); return rdr; }

	public static T Fill<T>(this XmlReader rdr) where T : ILoadXML, new() {
		var obj = rdr.LocalName;
		var rd = rdr.ReadSubtree();
		var ret = new T();
		if(rd.ReadToFollowing(obj)) ret.Load(rd);
		return ret;
	}

	public static void LoopAttr(this XmlReader rdr, Action<string,string,XmlReader> fnc){
		if(rdr.AttributeCount > 0){ rdr.MoveToFirstAttribute(); 
			do { if(rdr.HasValue) fnc(rdr.Name,rdr.Value,rdr); } while (rdr.MoveToNextAttribute());
		}
	}

	public static XmlWriter WriteElement(this XmlWriter wrt, string name, string? value, bool cdata=false){
		wrt.WriteStartElement(name);
		wrt.WriteAttributeString("xsi", "type", null, "xsd:string");
		if(cdata) wrt.WriteCData(value);
		else wrt.WriteString(value);
		wrt.WriteEndElement();
		return wrt;
	}

	public static string FCase(this string text){
		if (string.IsNullOrEmpty(text)) return string.Empty;
		var words = text.ToLower().Replace(",",", ").Replace("  "," ").Split(' ');
		return string.Join(" ", words.Select(word => word.Length > 0 ? char.ToUpper(word[0]) + word[1..] : word));
	}
}


