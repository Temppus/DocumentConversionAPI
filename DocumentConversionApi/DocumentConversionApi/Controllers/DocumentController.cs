using System;
using System.Threading;
using System.Threading.Tasks;
using Document.Conversion.DocumentStorage;
using DocumentConversionApi.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace DocumentConversionApi.Controllers
{
    [ApiController]
    [Route("Document")]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentConverterServiceFactory _converterServiceFactory;
        private readonly DocumentFileLoader _documentFileLoader;

        public DocumentController(DocumentConverterServiceFactory converterServiceFactory, DocumentFileLoader documentFileLoader)
        {
            _converterServiceFactory = converterServiceFactory ?? throw new ArgumentNullException(nameof(converterServiceFactory));
            _documentFileLoader = documentFileLoader ?? throw new ArgumentNullException(nameof(documentFileLoader));
        }

        [HttpPost("Convert")]
        [Produces("application/json", "application/xml")]
        public async Task<FileContentResult> ConvertAsync([FromForm] DocumentConvertRequest convertRequest, CancellationToken cancellationToken)
        {
            string fileContent;

            if (convertRequest.File != null)
            {
                await using var stream = convertRequest.File.OpenReadStream();
                fileContent = await _documentFileLoader.LoadFromStreamAsync(stream, cancellationToken);
            }
            else
            {
                fileContent = await _documentFileLoader.LoadDocumentFileAsync(convertRequest.InputFileProvider, convertRequest.FileSource, cancellationToken);
            }

            var documentConverterService = _converterServiceFactory.CreateDocumentConverterService(
                convertRequest.InputFileType,
                convertRequest.OutputFileType,
                convertRequest.OutputFileProvider);

            var convertedDocument = await documentConverterService.ConvertDocumentAsync(fileContent, convertRequest.Email, cancellationToken);
            
            return new FileContentResult(convertedDocument.Data, convertedDocument.ContentType)
            {
                FileDownloadName = convertedDocument.DocumentId,
            };
        }
    }
}
