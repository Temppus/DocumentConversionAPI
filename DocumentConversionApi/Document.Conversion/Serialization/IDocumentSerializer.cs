namespace Document.Conversion.Serialization
{
    public interface IDocumentSerializer
    {
        SerializedFile SerializeDocument(Document document);
    }
}
