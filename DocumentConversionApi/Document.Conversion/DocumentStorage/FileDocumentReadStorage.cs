using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Document.Conversion.DocumentStorage
{
    public interface IFileDocumentReadStorage : IDocumentReadStorage { }

    public class FileDocumentStorage : IFileDocumentReadStorage, IDocumentWriteStorage
    {
        private readonly string _baseDirPath;

        public FileDocumentStorage()
        {
            //TODO: Path should be injected
            _baseDirPath = Path.GetTempPath();
        }

        public Task<Stream> LoadDocumentAsync(string source, CancellationToken cancellationToken)
        {
            var fullFilepath = ValidateFilePath(source);

            if (!File.Exists(fullFilepath))
            {
                throw new FileNotFoundException(source);
            }

            return Task.FromResult((Stream)File.OpenRead(fullFilepath));
        }

        public async Task<string> SaveDocumentAsync(byte[] document, CancellationToken cancellationToken)
        {
            var documentId = Path.GetRandomFileName();
            var savePath = Path.Combine(_baseDirPath, documentId);

            var directoryPath = Path.GetDirectoryName(savePath);

            if (directoryPath == null)
                throw new DirectoryNotFoundException($"Failed to get directory from file path {Path.GetFullPath(savePath)}");

            Directory.CreateDirectory(directoryPath);

            await File.WriteAllBytesAsync(savePath, document, cancellationToken);
            return documentId;
        }

        private string ValidateFilePath(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                throw new ArgumentNullException(nameof(filepath));
            }

            var fullPath = Path.Combine(_baseDirPath, filepath);

            if (!fullPath.StartsWith(_baseDirPath, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Unsafe input file path");
            }

            return fullPath;
        }
    }
}
