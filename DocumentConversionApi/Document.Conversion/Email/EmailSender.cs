using System;
using System.Threading.Tasks;

namespace Document.Conversion.Email
{
    public class EmailSender : IEmailSender
    {
        public Task EnqueueEmailAsync(byte[] file, string toEmail, string fileName, string contentType)
        {
            // TODO: Ideally I would create and serialize message which would be pushed to some broker queue (and processed by another service)
            // TODO: or we could send email directly here if it is acceptable
            return Task.CompletedTask;
        }
    }
}
