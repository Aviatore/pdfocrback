using System.IO;
using Microsoft.AspNetCore.Http;

namespace PdfOcr.HelperMethods
{
    public static class FileFormatHelper
    {
        public static bool IsPdfFile(string filename)
        {
            return Path.GetExtension(filename).Equals(".pdf");
        }

        public static string SetExtension(string fullPath, string newExt)
        {
            var extension = Path.GetExtension(fullPath);
            
            return extension is null ? null : fullPath.Replace(extension, newExt);
        }
    }
}