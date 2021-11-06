using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Hosting;

namespace ExternalServerPrototype
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var cert = new X509Certificate2("certtest/private.pfx");

                    webBuilder.UseStartup<Startup>();

                    webBuilder.UseKestrel(options =>
                    {
                        options.ListenAnyIP(5001, listenOptions =>
                        {
                            listenOptions.UseHttps(cert, adapterOptions =>
                            {
                                adapterOptions.ServerCertificate = cert;
                                adapterOptions.CheckCertificateRevocation = false;
                
                                adapterOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                                adapterOptions.ClientCertificateValidation = (certificate2, chain, arg3) => true;

                            });
                        });
                            
                    });

                });
        }
    }
}