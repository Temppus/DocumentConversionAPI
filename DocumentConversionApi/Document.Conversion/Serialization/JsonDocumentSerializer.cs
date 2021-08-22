using System.Text;
using Newtonsoft.Json;

namespace Document.Conversion.Serialization
{
    public class JsonDocumentSerializer : IDocumentDeserializer, IDocumentSerializer
    {
        public Document Deserialize(string document)
        {
            return JsonConvert.DeserializeObject<Document>(document);
        }

        public SerializedFile SerializeDocument(Document document)
        {
            var s = JsonConvert.SerializeObject(document);

            return new SerializedFile
            {
                ContentType = System.Net.Mime.MediaTypeNames.Application.Json,
                Data = Encoding.UTF8.GetBytes(s)
            };
        }
    }
}
