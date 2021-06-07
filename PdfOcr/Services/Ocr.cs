using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace PdfOcr.Services
{
    public class Ocr : IOcr
    {
        private readonly ILogger<Ocr> _logger;

        public Ocr(ILogger<Ocr> logger)
        {
            _logger = logger;
        }
        public async void OcrPdfAsync(string fullPath)
        {
            using var proc = new Process();
            {
                proc.StartInfo = new ProcessStartInfo()
                {
                    FileName = "ocrmypdf",
                    Arguments = $"-q -l pol --force-ocr {fullPath} {fullPath.Replace(".pdf", "_ocr.pdf")}",
                    UseShellExecute = false,
                    //RedirectStandardOutput = true,
                    //RedirectStandardError = true,
                    CreateNoWindow = true
                };
            }
                
            _logger?.LogInformation("Start conversion");
            proc.Start();
            await proc.WaitForExitAsync();
            _logger?.LogInformation("Conversion finished");
        }
    }
}