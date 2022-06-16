using DistWF.Adapter.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace DistWF.Adapter
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();

            services.AddLogging();

            #region Detección de directorio de librerías compartidas e importación de ensamblados
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

        }

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DistWF Adapter API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
