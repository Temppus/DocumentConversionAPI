using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Document.Conversion.DocumentStorage
{
    public interface IDocumentReadStorage
    {
        /// <summary>
        /// Load document from defined source.
        /// </summary>
        /// <param name="source">Document source</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="InvalidDocumentSourceException">Document source is unsupported.</exception>
        /// <returns></returns>
        Task<Stream> LoadDocumentAsync(string source, CancellationToken cancellationToken);
    }
}
