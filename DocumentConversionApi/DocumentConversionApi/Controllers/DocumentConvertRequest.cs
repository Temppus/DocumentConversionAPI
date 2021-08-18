using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DocumentConversionApi.Controllers
{
    public enum FileType
    {
        Xml,
        Json,
    }

    public class DocumentConvertRequest
    {
        [Required]
        [EnumDataType(typeof(FileType), ErrorMessage = "unsupported FileType value")]
        public FileType FileType { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
