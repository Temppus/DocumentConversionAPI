using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Document.Conversion.DocumentConversion
{
    public interface IDocumentDeserializer
    {
        Task<Document> DeserializeFromXmlAsync(Stream stream, CancellationToken cancellationToken);
        Task<Document> DeserializeFromJsonAsync(Stream stream, CancellationToken cancellationToken);
    }
}
