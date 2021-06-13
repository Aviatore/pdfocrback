using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PdfOcr.Extensions;
using PdfOcr.HelperMethods;
using PdfOcr.Models;
using PdfOcr.Services;
using PdfOcr.SignalR;

namespace PdfOcr.Controllers
{
    [Route("Ocr")]
    [ApiController]
    public class GetFile : ControllerBase
    {
        private readonly IOcr _ocr;
        private object _locker;
        private ILogger<GetFile> _logger;

        public GetFile(IOcr ocr, ILogger<GetFile> logger)
        {
            _ocr = ocr;
            _locker = new object();
            _logger = logger;
        }
        
        [HttpPost("GetFile")]
        public async Task<IActionResult> Index(string connectionId)
        {
            FileFormatHelper.ClearDirectories();

            RequestResponse requestResponse = new RequestResponse()
            {
                Detail = "Something went wrong",
                Status = 500
            };
            string inputDirName = "InputFiles";
            string outputDirName = "OutputFiles";
            string fullDomainUrl = $"{Request.Scheme}{Uri.SchemeDelimiter}{Request.Host.ToString()}";

            var files = Request.Form.Files.Count > 0 ? Request.Form.Files : null;

            if (files is null)
            {
                requestResponse.Detail = "File was not retrieved";
                return Ok(requestResponse);
            }

            foreach (var file in files)
            {
                string inputFileNameTmp = $"{Guid.NewGuid()}.pdf";
                string outputFileNameTmp = $"{Guid.NewGuid()}.pdf";
            
                var inputFullPath = Path.Combine(inputDirName, inputFileNameTmp);

                var outputFullPath = Path.Combine(outputDirName, outputFileNameTmp);
                var outputFullUrl = Path.Combine(fullDomainUrl, outputDirName, outputFileNameTmp);
                
                if (!FileFormatHelper.IsPdfFile(file.FileName))
                {
                    requestResponse.Detail = "Sent file is not a pdf";
                    return Ok(requestResponse);
                }
            
                file.Save(inputFullPath);
                var f = Directory.GetFiles("InputFiles");
                var ff = String.Join(',', f);
                _logger.LogInformation($"Files: {ff}");
            
                await _ocr.OcrPdfAsync(inputFullPath, outputFullPath, outputFullUrl, connectionId, file.FileName);
            }
            
            requestResponse.Detail = "File was saved";
            requestResponse.Status = 200;
            
            return Ok(requestResponse);
        }
    }
}