using DistWF.Adapter.Infrastructure;
using DistWF.Common.Services;
using DistWF.Engine.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;

namespace DistWF.Adapter
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();

            services.AddLogging();
            services.AddScoped<ICalculationService, CalculationService>();

            string sharedLibPath = null;
            #region Detección de directorio de librerías compartidas
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
            #endregion

            var dllFiles = new DirectoryInfo(sharedLibPath).GetFiles("*.dll");
            foreach (var dllFileInfo in dllFiles)
            {
                Assembly backEndAssembly = Assembly.LoadFrom(dllFileInfo.FullName);
                services.InstallCalculationBackendsFromAssembly(backEndAssembly, Configuration);
            }
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
