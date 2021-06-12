using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
