using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PdfOcr.SignalR;

namespace PdfOcr.Services
{
    public class Ocr : IOcr
    {
        private readonly ILogger<Ocr> _logger;
        private readonly IHubContext<MyMessageHub> _hub;

        public Ocr(ILogger<Ocr> logger, IHubContext<MyMessageHub> hub)
        {
            _logger = logger;
            _hub = hub;
        }
        public async Task OcrPdfAsync(string inputFullPath, string outputFullPath, string outputFullUrl, string connectionId, string fileName)
        {
            if (connectionId is null)
            {
                await _hub.Clients.All.SendAsync("broadcastmessage", outputFullPath);
                return;
            }
            
            Console.WriteLine($"ConnectionId: {connectionId}");

            using var proc = new Process();
            {
                proc.StartInfo = new ProcessStartInfo()
                {
                    FileName = "ocrmypdf",
                    Arguments = $"-q -l pol --force-ocr {inputFullPath} {outputFullPath}",
                    UseShellExecute = false,
                    //RedirectStandardOutput = true,
                    //RedirectStandardError = true,
                    CreateNoWindow = true
                };
            }
                
            _logger?.LogInformation("Start conversion");
            proc.Start();
            await proc.WaitForExitAsync();
            await _hub.Clients.Client(connectionId).SendAsync("broadcastmessage", $"{outputFullUrl}:{fileName}");
            _logger?.LogInformation("Conversion finished");
        }
    }
}