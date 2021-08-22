using System;
using System.Threading;
using System.Threading.Tasks;
using Document.Conversion.DocumentStorage;
using Document.Conversion.Email;
using Document.Conversion.Serialization;

namespace Document.Conversion
{
    public class DocumentConverterService
    {
        private readonly IDocumentWriteStorage _documentWriteStorage;
        private readonly IEmailSender _emailSender;
        private readonly IDocumentSerializer _documentSerializer;
        private readonly IDocumentDeserializer _documentDeserializer;

        public DocumentConverterService(
            IDocumentSerializer documentSerializer,
            IDocumentDeserializer documentDeserializer,
            IDocumentWriteStorage documentWriteStorage,
            IEmailSender emailSender)
        {
            _documentSerializer = documentSerializer ?? throw new ArgumentNullException(nameof(documentSerializer));
            _documentDeserializer = documentDeserializer ?? throw new ArgumentNullException(nameof(documentDeserializer));
            _documentWriteStorage = documentWriteStorage ?? throw new ArgumentNullException(nameof(documentWriteStorage));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task<ConvertedFile> ConvertDocumentAsync(string fileContent, string emailAddress, CancellationToken cancellationToken)
        {
            var document = _documentDeserializer.Deserialize(fileContent);

            var serializedFile = _documentSerializer.SerializeDocument(document);

            var docId = await _documentWriteStorage.SaveDocumentAsync(serializedFile.Data, cancellationToken);

            if (emailAddress != null)
            {
                await _emailSender.EnqueueEmailAsync(serializedFile.Data, emailAddress, docId, serializedFile.ContentType);
            }

            return new ConvertedFile
            {
                Data = serializedFile.Data,
                ContentType = serializedFile.ContentType,
                DocumentId = docId
            };
        }
    }
}
