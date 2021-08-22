using System;
using System.Threading.Tasks;

namespace Document.Conversion.Email
{
    public interface IEmailSender
    {
        Task EnqueueEmailAsync(byte[] file, string toEmail, string fileName, string contentType);
    }
}
