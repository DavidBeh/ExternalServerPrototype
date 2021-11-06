using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;

namespace ExternalServerPrototype
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var cert = new X509Certificate2("certtest/private.pfx");
            var clientCert = new X509Certificate2("certtest/certificate.crt");

            
            services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate(options =>
            {
                options.ChainTrustValidationMode = X509ChainTrustMode.CustomRootTrust;
                options.CustomTrustStore = new X509Certificate2Collection(new []{cert, clientCert});
                options.ValidateCertificateUse = false;
                options.AllowedCertificateTypes = CertificateTypes.SelfSigned;

            });
            /*services.AddAuthentication(
                    CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate(options =>
                {
                    options.AllowedCertificateTypes = CertificateTypes.Chained;
                })
                // Adding an ICertificateValidationCache results in certificate auth caching the results.
                // The default implementation uses a memory cache.
                .AddCertificateCache();*/
            services.AddGrpc();

            /*services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate(options =>
            {
                //options.CustomTrustStore =
                //    new X509Certificate2Collection(new X509Certificate2("certtest/certificate.crt"));
                options.AllowedCertificateTypes = CertificateTypes.Chained;
                //options.RevocationMode = X509RevocationMode.NoCheck;
                options.ValidateCertificateUse = false;
                options.ValidateValidityPeriod = true;
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseAuthentication();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ExternalServerS>();

                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync(
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    });
            });
        }
    }
}