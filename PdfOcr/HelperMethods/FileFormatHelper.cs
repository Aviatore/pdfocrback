using System;
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

        public static void ClearDirectories()
        {
            Console.Out.WriteLine("Clearing directories ...");
            string[] directories = new[] {"InputFiles", "OutputFiles"};

            foreach (var directory in directories)
            {
                var inputFiles = Directory.EnumerateFiles(directory);
                foreach (var file in inputFiles)
                {
                    if ((DateTime.Now - File.GetCreationTime(file)) > TimeSpan.FromHours(5))
                    {
                        File.Delete(file);
                    }
                }
            }
        }
    }
}