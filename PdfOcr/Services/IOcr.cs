namespace PdfOcr.Services
{
    public interface IOcr
    {
        void OcrPdfAsync(string fullPath);
    }
}