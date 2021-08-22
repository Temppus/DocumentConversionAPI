using System.ComponentModel.DataAnnotations;
using Document.Conversion;
using Microsoft.AspNetCore.Http;

namespace DocumentConversionApi.Controllers
{
    public enum FileType
    {
        Xml,
        Json
    }

    public class DocumentConvertRequest
    {
        [Required]
        [EnumDataType(typeof(FileType), ErrorMessage = "Unsupported FileType value")]
        public FileType InputFileType { get; set; }

        [Required]
        public FileType OutputFileType { get; set; }

        public IFormFile File { get; set; }

        public string FileSource { get; set; }

        public InputFileProvider InputFileProvider { get; set; }

        [Required]
        public OutputFileProvider OutputFileProvider { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
