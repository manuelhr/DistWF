using DistWF.Common.Model;
using DistWF.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DistWF.Host.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void ImportTypesFromSharedAssemblies(this IServiceCollection services,
                                                                                               string sharedAssembliesDirectoryPath,
                                                                                               IConfiguration configuration)
        {
            var sharedDirFileInfo = new DirectoryInfo(sharedAssembliesDirectoryPath);
            if (sharedDirFileInfo.Exists == false) throw new Exception(Messages.AssemblyDirectoryNotFound);
            var assemblyFiles = new DirectoryInfo(sharedDirFileInfo.FullName).GetFiles("*.dll");
            if (assemblyFiles.Length == 0) throw new Exception(Messages.AssemblyDirectoryDoesNotContainAssemblies);

            services.InstallTypesFromEnginesAsembly(assemblyFiles.GetAssembly(DistWFAssemblyNames.Engine));
            services.InstallTypesFromBackEndAssembly(assemblyFiles.GetAssembly(DistWFAssemblyNames.BackEnd), configuration);
            services.InstallTypesFromAdapterAsembly(assemblyFiles.GetAssembly(DistWFAssemblyNames.Adapter));
        }

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

        static void InstallTypesFromAdapterAsembly(
        this IServiceCollection services,
        Assembly targetAssembly)
        {
            services.AddMvc().AddApplicationPart(targetAssembly);
        }

        static Assembly GetAssembly(this FileInfo[] assemblyFiles, string targetAssemblyName)
        {
            var assemblyFileInfo = assemblyFiles.FirstOrDefault(x => string.Equals(x.Name,
                                                                                                                      targetAssemblyName,
                                                                                                                      StringComparison.OrdinalIgnoreCase));
            if (assemblyFileInfo == null) throw new Exception($"{Messages.AssemblyNotFound} ({targetAssemblyName}).");
            return Assembly.LoadFrom(assemblyFileInfo.FullName);
        }
    }
}
