using System.Threading.Tasks;

namespace PdfOcr.Services
{
    public interface IOcr
    {
        Task OcrPdfAsync(string inputFullPath, string outputFullPath, string outputFullUrl, string connectionId, string fileName);
    }
}