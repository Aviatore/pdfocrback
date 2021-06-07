using System.IO;
using Microsoft.AspNetCore.Http;

namespace PdfOcr.Extensions
{
    public static class Extension
    {
        public static void Save (this IFormFile file, string fullPath)
        {
            using var stream = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(stream);
        }
    }
}