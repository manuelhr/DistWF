using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DistWF.Adapter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(x => x.AddCommandLine(args))
                .ConfigureWebHostDefaults(webBuilder =>
                        webBuilder.UseStartup<Startup>());

            return builder;
         
        }
    }
}
