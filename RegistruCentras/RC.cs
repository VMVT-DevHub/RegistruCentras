using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using RC.Classes;
using RC.Services;


namespace RC;

public class RegistruCentras(string cert, string pass) {
	public string? User { get; set; }
	public string? Url { get; set; }
	private RSA Key { get; set; } = new X509Certificate2(Convert.FromBase64String(cert), pass, X509KeyStorageFlags.MachineKeySet).GetRSAPrivateKey() ?? throw new("Certificate does not have private Key");
	public RCDataRequest Sign(RCDataRequest req){
		req.Signature = Convert.ToBase64String(Key.SignData(req.GetBytes(), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
		return req;
	}
	public JAR GetJAR() => new (this);
}


