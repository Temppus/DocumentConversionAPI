using System;
using System.Threading;
using System.Threading.Tasks;
using Document.Conversion;
using Document.Conversion.DocumentConversion;
using DocumentConversionApi.Validation;
using Microsoft.AspNetCore.Mvc;

namespace DocumentConversionApi.Controllers
{
    [ApiController]
    [Route("Document")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IDocumentDeserializer _documentDeserializer;

        public DocumentController(IDocumentService documentService, IDocumentDeserializer documentDeserializer)
        {
            _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
            _documentDeserializer = documentDeserializer ?? throw new ArgumentNullException(nameof(documentDeserializer));
        }

        [HttpPost("Convert")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> ConvertAsync([FromForm] DocumentConvertRequest convertRequest, CancellationToken cancellationToken)
        {
            var headers = Request.Headers;
            
            await using var stream = convertRequest.File.OpenReadStream();

            Document.Conversion.Document doc;

            switch (convertRequest.FileType)
            {
                case FileType.Json:
                    doc = await _documentDeserializer.DeserializeFromJsonAsync(stream, cancellationToken);
                    break;
                case FileType.Xml:
                    doc = await _documentDeserializer.DeserializeFromXmlAsync(stream, cancellationToken);
                    break;
                default:
                    throw new FileValidationException($"Unsupported {nameof(FileType)}");
            }

            return Ok(doc);
            
        }


        /*[HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFileAsync(IFormFile file, CancellationToken cancellationToken)
        {
            if (CheckIfExcelFile(file))
            {
                await WriteFile(file);
            }
            else
            {
                return BadRequest(new { message = "Invalid file extension" });
            }

            return Ok();
        }*/
    }
}
