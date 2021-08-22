using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Document.Conversion.DocumentStorage
{
    public interface IHttpDocumentReadStorage : IDocumentReadStorage { }

    public class HttpDocumentReadStorage : IHttpDocumentReadStorage
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpDocumentReadStorage(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<Stream> LoadDocumentAsync(string source, CancellationToken cancellationToken)
        {
            if (!Uri.TryCreate(source, UriKind.Absolute, out var uri))
            {
                throw new InvalidDocumentSourceException($"Source {source} is not valid URI.");
            }

            if (uri.Scheme != Uri.UriSchemeHttp)
            {
                throw new InvalidDocumentSourceException($"Uri {source} scheme is not HTTP.");
            }

            var client =_httpClientFactory.CreateClient();
            return await client.GetStreamAsync(uri, cancellationToken);
        }
    }
}
