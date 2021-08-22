using System;

namespace Document.Conversion.DocumentStorage
{
    public class InvalidDocumentSourceException : Exception
    {
        public InvalidDocumentSourceException(string message) : base(message) { }
    }
}
