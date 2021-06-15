using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PdfOcr.HelperMethods;

namespace PdfOcr
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!Directory.Exists("OutputFiles"))
                Directory.CreateDirectory("OutputFiles");
            
            if (!Directory.Exists("InputFiles"))
                Directory.CreateDirectory("InputFiles");
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(log =>
                {
                    log.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
                    log.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
