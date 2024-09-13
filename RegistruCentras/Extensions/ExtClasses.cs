using System.Text;
using System.Xml;

namespace RC.Extensions;

public class Base64Stream(XmlReader reader) : Stream {
	public override bool CanRead => true;
	public override bool CanSeek => false;
	public override bool CanWrite => false;
	public override long Length => throw new NotImplementedException();
	public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public override void Flush() { throw new NotImplementedException(); }
	public override int Read(byte[] buffer, int offset, int count) => Reader.ReadElementContentAsBase64(buffer,offset,count);
	public override long Seek(long offset, SeekOrigin origin) { throw new NotImplementedException(); }
	public override void SetLength(long value) { throw new NotImplementedException(); }
	public override void Write(byte[] buffer, int offset, int count) { throw new NotImplementedException(); }
	private XmlReader Reader { get; set; } = reader;
}

public class Utf8StringWriter : StringWriter {
	public override Encoding Encoding => Encoding.UTF8;
}



public interface ILoadXML {
	protected virtual void LoadAttr(string key, string val, XmlReader rdr) { Console.WriteLine($"{GetType().FullName}: This should not happen."); }
	protected virtual void LoadData(string key, XmlReader rdr){ Console.WriteLine($"{GetType().FullName}: This should not happen."); }
	protected virtual void LoadModif(){}

	public void Load(XmlReader rdr){
		if(rdr.HasAttributes){ rdr.MoveToFirstAttribute(); do { if(rdr.HasValue) LoadAttr(rdr.Name,rdr.Value,rdr); } while (rdr.MoveToNextAttribute()); }
		if(!rdr.IsEmptyElement)
			while (rdr.Read()) 
				if(rdr.IsStartElement())
					LoadData(rdr.LocalName, rdr);
		LoadModif();
	}
}