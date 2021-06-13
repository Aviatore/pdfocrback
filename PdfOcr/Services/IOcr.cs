using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace PdfOcr.Services
{
    public interface IOcr
    {
        Task OcrPdfAsync(string inputFullPath, string outputFullPath, string outputFullUrl, string connectionId, string fileName);
        //public bool KillProcess(string connectionId);
        public bool KillProcess(HubCallerContext context);
    }
}