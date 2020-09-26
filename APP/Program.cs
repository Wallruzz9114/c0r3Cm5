using System;
using System.Diagnostics;
using System.IO;
using LoggingService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace APP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;

                try
                {
                    Console.WriteLine("Serilog Test");
                }
                catch (Exception exception)
                {
                    Log.Error(
                        $"An error occurred while testing Serilog " +
                        $"{exception.Message} {exception.StackTrace} " +
                        $"{exception.InnerException} {exception.Source}"
                    );
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                // Serilog configuration
                webBuilder.UseSerilog((hostingContext, loggingConfiguration) => loggingConfiguration
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", "c0r3Cm5")
                    .Enrich.WithProperty("MachineName", Environment.MachineName)
                    .Enrich.WithProperty("CurrentManagedThreadId", Environment.CurrentManagedThreadId)
                    .Enrich.WithProperty("OSVersion", Environment.OSVersion)
                    .Enrich.WithProperty("Version", Environment.Version)
                    .Enrich.WithProperty("UserName", Environment.UserName)
                    .Enrich.WithProperty("ProcessId", Process.GetCurrentProcess().Id)
                    .Enrich.WithProperty("ProcessName", Process.GetCurrentProcess().ProcessName)
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                    .WriteTo.File(
                        formatter: new LogTextFormatter(),
                        path: Path.Combine(
                            hostingContext.HostingEnvironment.ContentRootPath +
                            $"{Path.DirectorySeparatorChar}Logs{Path.DirectorySeparatorChar}",
                            $"c0r3Cm5_log_{DateTime.Now:yyyyMMdd}.txt"
                        )
                    )
                    .ReadFrom.Configuration(hostingContext.Configuration)
                );

                webBuilder.UseStartup<Startup>();
            });
    }
}
