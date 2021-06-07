using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfOcr.Extensions;
using PdfOcr.HelperMethods;
using PdfOcr.Models;
using PdfOcr.Services;

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
        public IActionResult Index()
        {
            RequestResponse requestResponse = new RequestResponse()
            {
                Detail = "Something went wrong",
                Status = 500
            };
            string dirPath = "InputFiles";
            var fullPath = Path.Combine(dirPath, "input.pdf");
            
            IFormFile file = Request.Form.Files.Count > 0 ? Request.Form.Files[0] : null;

            if (file is null)
            {
                requestResponse.Detail = "File was not retrieved";
                return Ok(requestResponse);
            }

            if (!FileFormatHelper.IsPdfFile(file.FileName))
            {
                requestResponse.Detail = "Sent file is not a pdf";
                return Ok(requestResponse);
            }
            
            file.Save(fullPath);
            
            _ocr.OcrPdfAsync(fullPath);

            requestResponse.Detail = "File was saved";
            requestResponse.Status = 200;
            
            return Ok(requestResponse);
        }
    }
}