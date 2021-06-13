using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using PdfOcr.SignalR;

namespace PdfOcr.Services
{
    public class Ocr : IOcr
    {
        private readonly ILogger<Ocr> _logger;
        private readonly IHubContext<MyMessageHub> _hub;
        private Dictionary<string, Process> _processes;
        private bool _killed;

        public Ocr(ILogger<Ocr> logger, IHubContext<MyMessageHub> hub)
        {
            _logger = logger;
            _hub = hub;
            _processes = new Dictionary<string, Process>();
        }
        public async Task OcrPdfAsync(string inputFullPath, string outputFullPath, string outputFullUrl, string connectionId, string fileName)
        {
            _killed = false;
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
                
            _logger.LogInformation($"[{DateTime.Now:HH:mm:ss}] Start conversion");
            _processes.Add(connectionId, proc);
            proc.Start();
            await proc.WaitForExitAsync();

            if (!_killed)
            {
                await _hub.Clients.Client(connectionId).SendAsync("broadcastmessage", $"{outputFullUrl}:{fileName}");
            }
            
            _processes.Remove(connectionId);
            _logger.LogInformation($"[{DateTime.Now:HH:mm:ss}] Conversion finished");
            _logger.LogInformation($"[{DateTime.Now:HH:mm:ss}] Number of processes: {_processes.Count} running by {connectionId}");
        }

        /*public bool KillProcess(string connectionId)
        {
            if (_processes.ContainsKey(connectionId))
            {
                _logger.LogInformation($"[{DateTime.Now:HH:mm:ss}] Killing process of {connectionId}");
                _processes[connectionId].Kill(true);
            }

            return true;
        }*/
        
        public bool KillProcess(HubCallerContext context)
        {
            if (_processes.ContainsKey(context.ConnectionId))
            {
                _logger.LogInformation($"[{DateTime.Now:HH:mm:ss}] Killing process of {context.ConnectionId}");
                _processes[context.ConnectionId].Kill(true);
                _killed = true;
            }

            return true;
        }
    }
}