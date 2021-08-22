using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Document.Conversion.DocumentStorage
{
    public interface ICloudDocumentReadStorage : IDocumentReadStorage { }

    public class CloudDocumentStorage : ICloudDocumentReadStorage, IDocumentWriteStorage
    {
        // TODO: Inject some auth/config values for remote cloud storage provider
        public CloudDocumentStorage()
        {

        }

        public Task<Stream> LoadDocumentAsync(string source, CancellationToken cancellationToken)
        {
            // TODO: Sanitize
            // TODO: Download file from cloud storage via some library
            throw new System.NotImplementedException();
        }

        public Task<string> SaveDocumentAsync(byte[] document, CancellationToken cancellationToken)
        {
            // TODO: Upload file to configured cloud storage
            // TODO: Return id of uploaded file
            throw new System.NotImplementedException();
        }
    }
}
