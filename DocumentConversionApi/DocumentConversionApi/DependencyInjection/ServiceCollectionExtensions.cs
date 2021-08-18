using Document.Conversion;
using Document.Conversion.DocumentConversion;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentConversionApi.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentConversionServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDocumentService, DocumentService>();
            serviceCollection.AddTransient<IDocumentDeserializer, DocumentDeserializer>();

            return serviceCollection;
        }
    }
}
