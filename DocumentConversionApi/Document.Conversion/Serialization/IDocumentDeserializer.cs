namespace Document.Conversion.Serialization
{
    public interface IDocumentDeserializer
    {
        Document Deserialize(string document);
    }
}
