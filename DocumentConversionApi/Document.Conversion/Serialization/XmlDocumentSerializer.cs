using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Document.Conversion.Serialization
{
    public class XmlDocumentSerializer : IDocumentDeserializer, IDocumentSerializer
    {
        private static readonly XmlSerializer XmlSerializer = new(typeof(Document));

        public Document Deserialize(string document)
        {
            var xDocument = XDocument.Parse(document);
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

        public SerializedFile SerializeDocument(Document document)
        {
            using var stringWriter = new StringWriter();
            using var writer = XmlWriter.Create(stringWriter);
            XmlSerializer.Serialize(writer, document);

            return new SerializedFile
            {
                ContentType = System.Net.Mime.MediaTypeNames.Application.Xml,
                Data = Encoding.UTF8.GetBytes(stringWriter.ToString())
            };
        }
    }
}
