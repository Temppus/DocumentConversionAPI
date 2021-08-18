using System;
using System.Threading;
using System.Threading.Tasks;
using Document.Conversion.DocumentConversion;
using DocumentConversionApi.Validation;
using Microsoft.AspNetCore.Mvc;

namespace DocumentConversionApi.Controllers
{
    [ApiController]
    [Route("Document")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentDeserializer _documentDeserializer;

        public DocumentController(IDocumentDeserializer documentDeserializer)
        {
            _documentDeserializer =
                documentDeserializer ?? throw new ArgumentNullException(nameof(documentDeserializer));
        }

        [HttpPost("Convert")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> ConvertAsync([FromForm] DocumentConvertRequest convertRequest,
            CancellationToken cancellationToken)
        {
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
    }
}
