using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


// This application creates certificates from the RSS keys in the project, that get copied to output.
// The private.pem is the key used, private1.pem is a different key. 
namespace CertCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(File.ReadAllText("private.pem").ToCharArray());
            var csr = new CertificateRequest("cn=test", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var x509 = csr.CreateSelfSigned(DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddDays(300));
            var pubBytes = x509.Export(X509ContentType.Cert);
            var privBytes = x509.Export(X509ContentType.Pkcs12);
            Directory.CreateDirectory("generated");
            File.WriteAllBytes("generated/private.pfx", privBytes);
            File.WriteAllBytes("generated/certificate.crt", pubBytes);
            var path = Path.GetFullPath("generated");
            Process.Start("explorer.exe", path);
        }
    }
}