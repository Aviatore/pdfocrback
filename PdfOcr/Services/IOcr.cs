namespace PdfOcr.Services
{
    public interface IOcr
    {
        void OcrPdfAsync(string inputFullPath, string outputFullPath, string outputFullUrl, string connectionId);
    }
}