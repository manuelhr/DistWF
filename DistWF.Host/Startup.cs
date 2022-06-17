using DistWF.Host.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace DistWF.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string adapterURL = Configuration.GetValue<string>("AdapterURL");

            services.AddHttpClient("Adapter", client =>
            {
                client.BaseAddress = new Uri(adapterURL);
            })
        ;
            services.AddLogging();

            #region Detecci�n de directorio de librer�as compartidas e importaci�n de ensamblados
            string sharedLibPath = null;
            var sharedLibPathConfigValue = Configuration.GetValue<string>("SharedLibPath") ?? "DistWF.SharedLibs";
            if (Directory.Exists(sharedLibPathConfigValue))
            {
                sharedLibPath = sharedLibPathConfigValue;
            }
            else
            {
                string rootSolutionPath = new DirectoryInfo(Environment.CurrentDirectory).Parent.FullName;
                sharedLibPath = Path.Combine(rootSolutionPath, sharedLibPathConfigValue);
            }
            services.ImportTypesFromSharedAssemblies(sharedLibPath, Configuration);
            #endregion
            
            services.AddControllers();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DistWF Host API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
