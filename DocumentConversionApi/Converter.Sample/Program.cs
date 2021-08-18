using System.Threading.Tasks;

namespace Converter.Sample
{
    internal class Program
    {
        internal static async Task Main()
        {
            //OriginalSample.RunConversion();
            await RefactoredSample.RunConversionAsync();
        }
    }
}