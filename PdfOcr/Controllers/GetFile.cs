using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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

        public GetFile(IOcr ocr)
        {
            _ocr = ocr;
        }
        
        [HttpPost("GetFile")]
        public IActionResult Index(string connectionId)
        {
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
            
                _ocr.OcrPdfAsync(inputFullPath, outputFullPath, outputFullUrl, connectionId);
            }
            
            requestResponse.Detail = "File was saved";
            requestResponse.Status = 200;
            
            return Ok(requestResponse);
        }
    }
}