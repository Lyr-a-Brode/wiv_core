using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using WivCore.Utils.Logging;

namespace WivCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .MinimumLevel.Is(LogEventLevel.Debug)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(new RenderedJsonFormatter()))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(serverOptions =>
                        {
                            serverOptions.Listen(IPAddress.Any, 8000);
                        })
                        .UseStartup<Startup>();
                });
    }
}