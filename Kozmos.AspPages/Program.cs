using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Kozmos.AspPages
{
    public class Program
    {

        public static void Main(string[] args)
        {
            SetupSerilog();

            try
            {
                Log.Information("Host starting...");
                CreateHostBuilder(args).Build().Run();
                Log.Information("Host started");

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Server failed to start");
            }
        }

        private static void SetupSerilog()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(configuration)
              .CreateLogger();

            Log.Information("Hello, world!");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>()
                    .UseSerilog(logger:Log.Logger);
                 
                });
    }
}
