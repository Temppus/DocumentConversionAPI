using Document.Conversion.DocumentStorage;
using Document.Conversion.Email;
using Document.Conversion.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentConversionApi.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentConversionServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<DocumentConverterServiceFactory>();

            serviceCollection.AddScoped<JsonDocumentSerializer>();
            serviceCollection.AddScoped<XmlDocumentSerializer>();

            serviceCollection.AddScoped<JsonDocumentSerializer>();
            serviceCollection.AddScoped<XmlDocumentSerializer>();

            serviceCollection.AddScoped<IEmailSender, EmailSender>();

            serviceCollection.AddScoped<IFileDocumentReadStorage, FileDocumentStorage>();
            serviceCollection.AddScoped<ICloudDocumentReadStorage, CloudDocumentStorage>();
            serviceCollection.AddScoped<IHttpDocumentReadStorage, HttpDocumentReadStorage>();

            serviceCollection.AddScoped<FileDocumentStorage>();
            serviceCollection.AddScoped<CloudDocumentStorage>();

            serviceCollection.AddScoped<DocumentFileLoader>();

            return serviceCollection;
        }
    }
}
