using System.Threading;
using System.Threading.Tasks;

namespace Document.Conversion.DocumentStorage
{
    public interface IDocumentWriteStorage
    {
        /// <summary>
        /// Saves the file document to underlying storage and returns id of saved file.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> SaveDocumentAsync(byte[] document, CancellationToken cancellationToken);
    }
}
