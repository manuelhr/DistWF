using DistWF.Common.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace DistWF.Adapter.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void InstallCalculationBackendsFromAssembly(
          this IServiceCollection services,
          Assembly targetAssembly,
          IConfiguration configuration)
        {
            var calculationBackEndsTypesInAssembly = targetAssembly.ExportedTypes
                                        .Where(x => typeof(ICalculationBackend)
                                                                .IsAssignableFrom(x) &&
                                                                !x.IsInterface &&
                                                                !x.IsAbstract)
                                        .ToList();

            string backEndName = configuration.GetValue<string>("config:backEndName");

            foreach (var calcBackEndType in calculationBackEndsTypesInAssembly)
            {
                services.AddScoped<ICalculationBackend>(x =>
                {
                    return (ICalculationBackend)Activator.CreateInstance(calcBackEndType,
                                                                                                            new object[] {
                                                                                                                backEndName?? "defaultBackEndName",
                                                                                                                x.GetRequiredService<ICalculationService>(),
                                                                                                                x.GetRequiredService<ILogger<Startup>>()
                                                                                                                });

                });
            }
        }

    }
}
