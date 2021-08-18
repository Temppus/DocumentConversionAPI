using System;

namespace DocumentConversionApi.Validation
{
    public class FileValidationException : Exception
    {
        public FileValidationException(string message) : base(message) { }
    }
}
