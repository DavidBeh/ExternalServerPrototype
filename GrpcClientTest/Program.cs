using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using ExternalServerPrototype;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;

namespace GrpcClientTest
{

    class Program
    { 
        static void Main(string[] args)
        {


                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                // I am not sure if I need this. When removed I get this:
                // HttpRequestException: The SSL connection could not be established, see inner exception.
                // AuthenticationException: The remote certificate is invalid according to the validation procedure:
                // RemoteCertificateNameMismatch, RemoteCertificateChainErrors
                handler.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;
                // Uses certificate.crt file

                var crt = new X509Certificate2("certificate1.crt");
                handler.ClientCertificates.Add(crt);

            var options = new GrpcChannelOptions()
            {
                HttpHandler = handler,
            };
            var channel = GrpcChannel.ForAddress("https://localhost:5001", options);
            
            var client = new ExternalServerService.ExternalServerServiceClient(channel);
            var request = new RepeatRequest
            {
                Repeats = 3,
                Words = "test"
            };

            var reply = client.RepeatWords(request, new Metadata
            {
                {"somekey", "somevalue"}
            });
            
            
            
            Console.WriteLine(reply.RepeatedWords);
        }
    }

    internal class TestHandler : HttpClientHandler
    {


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            
            Console.WriteLine(request.Headers.ToString());
            return await base.SendAsync(request, cancellationToken);
        }
    }
}