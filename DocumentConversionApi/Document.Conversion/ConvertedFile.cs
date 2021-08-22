namespace Document.Conversion
{
    public class ConvertedFile
    {
        public string DocumentId { get; init; }
        public byte[] Data { get; init; }
        public string ContentType { get; init; }
    }
}
