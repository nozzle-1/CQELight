using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using CQELight;

namespace CQELight_ASPNETCore3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureCQELight((b, config) =>
                {
                    var conf = config["conf"]; // Access to configuration right here for connection strings or so
                    b.UseAutofacAsIoC();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
