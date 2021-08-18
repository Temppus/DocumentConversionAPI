using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Document.Conversion.DocumentConversion
{
    public class DocumentDeserializer : IDocumentDeserializer
    {
        public async Task<Document> DeserializeFromXmlAsync(Stream stream, CancellationToken cancellationToken)
        {
            var xDocument = await XDocument.LoadAsync(stream, LoadOptions.None, cancellationToken);
            var root = xDocument.Root;

            if (root == null)
                throw new ArgumentException("XML document does not contain root node.");

            const string titleNodeName = "title";
            const string textNodeName = "text";

            var titleNode = root.Element(titleNodeName);

            if (titleNode == null)
                throw new ArgumentException($"Xml document does not contain required node with name {titleNodeName}");

            var textNode = root.Element(textNodeName);

            if (textNode == null)
                throw new ArgumentException($"Xml document does not contain required node with name {textNodeName}");

            // Is it required for title/text to be non null values ?
            var doc = new Document
            {
                Title = titleNode.Value,
                Text = textNode.Value
            };

            return doc;
        }

        public Task<Document> DeserializeFromJsonAsync(Stream stream, CancellationToken cancellationToken)
        {
            using var streamReader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(streamReader);
            var serializer = new JsonSerializer();
            return Task.FromResult(serializer.Deserialize<Document>(jsonReader));
        }
    }
}