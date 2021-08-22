using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Document.Conversion.DocumentStorage
{
    public class DocumentFileLoader
    {
        private readonly IFileDocumentReadStorage _fileDocumentReadStorage;
        private readonly IHttpDocumentReadStorage _httpDocumentReadStorage;
        private readonly ICloudDocumentReadStorage _cloudDocumentReadStorage;

        public DocumentFileLoader(IFileDocumentReadStorage fileDocumentReadStorage, IHttpDocumentReadStorage httpDocumentReadStorage, ICloudDocumentReadStorage cloudDocumentReadStorage)
        {
            _fileDocumentReadStorage = fileDocumentReadStorage ?? throw new ArgumentNullException(nameof(fileDocumentReadStorage));
            _httpDocumentReadStorage = httpDocumentReadStorage ?? throw new ArgumentNullException(nameof(httpDocumentReadStorage));
            _cloudDocumentReadStorage = cloudDocumentReadStorage ?? throw new ArgumentNullException(nameof(cloudDocumentReadStorage));
        }

        public Task<string> LoadFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            return ReadFileStreamSafe(stream, cancellationToken);
        }

        public async Task<string> LoadDocumentFileAsync(InputFileProvider provider, string source, CancellationToken cancellationToken)
        {
            Stream stream;

            switch (provider)
            {
                case InputFileProvider.File:
                    stream = await _fileDocumentReadStorage.LoadDocumentAsync(source, cancellationToken);
                    break;
                case InputFileProvider.Http:
                    stream = await _httpDocumentReadStorage.LoadDocumentAsync(source, cancellationToken);
                    break;
                case InputFileProvider.Cloud:
                    stream = await _cloudDocumentReadStorage.LoadDocumentAsync(source, cancellationToken);
                    break;
                default:
                    throw new NotSupportedException(provider.ToString());
            }

            return await ReadFileStreamSafe(stream, cancellationToken);
        }


        private static async Task<string> ReadFileStreamSafe(Stream stream, CancellationToken cancellationToken)
        {
            using var sr = new StreamReader(stream);
            // TODO: We should definitely limit max size of file to be read here (read partially and throw when above limit)
            // TODO: Prevent big files download attack
            var documentFile = await sr.ReadToEndAsync();
            return documentFile;
        }
    }
}
