using System;
using Document.Conversion;
using Document.Conversion.DocumentStorage;
using Document.Conversion.Email;
using Document.Conversion.Serialization;
using DocumentConversionApi.Controllers;

namespace DocumentConversionApi.DependencyInjection
{
    public class DocumentConverterServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DocumentConverterServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public DocumentConverterService CreateDocumentConverterService(FileType inputFileType, FileType outputFileType, OutputFileProvider outputFileProvider)
        {
            IDocumentDeserializer documentDeserializer = inputFileType switch
            {
                FileType.Json => (IDocumentDeserializer) _serviceProvider.GetService(typeof(JsonDocumentSerializer)),
                FileType.Xml => (IDocumentDeserializer) _serviceProvider.GetService(typeof(XmlDocumentSerializer)),
                _ => throw new NotSupportedException($"Unsupported input {nameof(FileType)} {inputFileType}")
            };

            IDocumentSerializer documentSerializer = outputFileType switch
            {
                FileType.Json => (IDocumentSerializer)_serviceProvider.GetService(typeof(JsonDocumentSerializer)),
                FileType.Xml => (IDocumentSerializer)_serviceProvider.GetService(typeof(XmlDocumentSerializer)),
                _ => throw new NotSupportedException($"Unsupported output {nameof(FileType)} {inputFileType}")
            };

            IDocumentWriteStorage documentWriteStorage = outputFileProvider switch
            {
                OutputFileProvider.File => (IDocumentWriteStorage)_serviceProvider.GetService(typeof(FileDocumentStorage)),
                OutputFileProvider.Cloud => (IDocumentWriteStorage)_serviceProvider.GetService(typeof(CloudDocumentStorage)),
                _ => throw new NotSupportedException($"Unsupported {nameof(OutputFileProvider)} {outputFileProvider}")
            };

            var service = new DocumentConverterService(
                documentSerializer,
                documentDeserializer,
                documentWriteStorage,
                (IEmailSender)_serviceProvider.GetService(typeof(IEmailSender)));

            return service;
        }
    }
}
