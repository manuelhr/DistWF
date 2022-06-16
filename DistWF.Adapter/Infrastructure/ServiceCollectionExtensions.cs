using DistWF.Common.Model;
using DistWF.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DistWF.Adapter.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        static void InstallTypesFromBackEndAssembly(
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

        static void InstallTypesFromEnginesAsembly(
          this IServiceCollection services,
          Assembly targetAssembly)
        {
            var typesInAssembly = targetAssembly.ExportedTypes
                                        .Where(x => typeof(ICalculationService)
                                                                .IsAssignableFrom(x) &&
                                                                !x.IsInterface &&
                                                                !x.IsAbstract)
                                        .ToList();

            foreach (var type in typesInAssembly)
            {
                services.AddScoped<ICalculationService>(x =>
                {
                    return (ICalculationService)Activator.CreateInstance(type);
                });
            }
        }

        public static void ImportTypesFromSharedAssemblies(this IServiceCollection services,
                                                                                                string sharedAssembliesDirectoryPath,
                                                                                                IConfiguration configuration)
        {
            var sharedDirFileInfo = new DirectoryInfo(sharedAssembliesDirectoryPath);
            if (sharedDirFileInfo.Exists == false) throw new Exception("Directorio de ensamblados compartidos no encontrado.");
            var assemblyFiles = new DirectoryInfo(sharedDirFileInfo.FullName).GetFiles("*.dll");
            if (assemblyFiles.Length == 0) throw new Exception("No se encontró ensamblados en el directorio compartido.");

            #region 1) DistWF.Engine
            var engineAssemblyFileInfo = assemblyFiles.FirstOrDefault(x => string.Equals(x.Name,
                                                                                                                    DistWFAssemblyNames.Engine,
                                                                                                                    StringComparison.OrdinalIgnoreCase));
            if (engineAssemblyFileInfo == null) throw new Exception($"Ensamblado '{DistWFAssemblyNames.Engine}' no encontrado.");
            Assembly engineAssembly = Assembly.LoadFrom(engineAssemblyFileInfo.FullName);
            services.InstallTypesFromEnginesAsembly(engineAssembly);
            #endregion
            #region 2) DistWF.Backend
            var backendAssemblyFileInfo = assemblyFiles.FirstOrDefault(x => string.Equals(x.Name,
                                                                                                                   DistWFAssemblyNames.BackEnd,
                                                                                                                   StringComparison.OrdinalIgnoreCase));
            Assembly backendAssembly = Assembly.LoadFrom(backendAssemblyFileInfo.FullName);
            if (backendAssembly == null) throw new Exception($"Ensamblado '{DistWFAssemblyNames.BackEnd}' no encontrado.");
            services.InstallTypesFromBackEndAssembly(backendAssembly, configuration);
            #endregion

        }

    }
}
