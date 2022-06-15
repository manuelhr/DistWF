using DistWF.Common.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DistWF.Controller
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Bienvenido a DistWF.Controller App");
            var host = CreateHostBuilder(args).Build();
            var logger = host.Services.GetService<ILogger<Program>>();
            while (true)
            {
                var operands = TryGetOperands();
                var appService = host.Services.GetService<DistWFControllerApp>();
                var response = await appService.Calculate(new Common.Model.CalculationRequest()
                {
                    Operand1 = operands.Item1,
                    ServiceName = operands.Item2,
                    Operand2 = operands.Item3
                });

                if (response.Success)
                {
                    logger.LogWarning(JsonConvert.SerializeObject(response, Formatting.Indented));
                }
                else
                {
                    logger.LogError(JsonConvert.SerializeObject(response, Formatting.Indented));
                }
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();

            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddHttpClient("default", client =>
                    {
                        client.BaseAddress = new Uri(configuration["ServerURL"]);
                    });
                    services.AddLogging();
                    services.AddTransient<DistWFControllerApp>();
                });

            return hostBuilder;
        }
        static Tuple<decimal, string, decimal> TryGetOperands()
        {
            decimal operand1 = 0, operand2 = 0;
            string operationName = null;
            bool operand1HasValue = false, operationNameHasValue = false, operand2HasValue = false;
            #region 1) Lectura del primer operando
            while (!operand1HasValue)
            {
                Console.WriteLine("Por favor, ingrese el primer operando:");
                string tmpOperand1 = Console.ReadLine();
                operand1HasValue = decimal.TryParse(tmpOperand1, out operand1);
            }
            #endregion

            #region Lectura de la operación
            while (!operationNameHasValue)
            {
                Console.WriteLine("Por favor, ingrese la operación ( +, - , * , / )");
                string tmpOperation = Console.ReadLine();
                switch (tmpOperation)
                {
                    case "+":
                        operationName = CalculationServiceNames.Sum;
                        break;
                    case "-":
                        operationName = CalculationServiceNames.Substract;
                        break;
                    case "*":
                        operationName = CalculationServiceNames.Multiply;
                        break;
                    case "/":
                        operationName = CalculationServiceNames.Divide;
                        break;
                    default:
                        operationName = null;
                        break;
                }
                operationNameHasValue = !string.IsNullOrWhiteSpace(operationName);
            }
            #endregion

            #region Lectura del segundo operando
            while (!operand2HasValue)
            {
                Console.WriteLine("Por favor, ingrese el segundo operando:");
                string tmpOperand2 = Console.ReadLine();
                operand2HasValue = decimal.TryParse(tmpOperand2, out operand2);
            }
            #endregion

            return new Tuple<decimal, string, decimal>(operand1, operationName, operand2);
        }
    }
}
