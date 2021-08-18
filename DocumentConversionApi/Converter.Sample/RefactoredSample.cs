using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Converter.Sample
{
    internal static class RefactoredSample
    {
        private class Document
        {
            public string Title { get; init; }
            public string Text { get; init; }
        }

        internal static async Task RunConversionAsync()
        {
            var sourceFileName = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Source Files\Document1.xml");
            var targetFileName = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Target Files\Document1.json");

            if (!File.Exists(sourceFileName))
                throw new FileNotFoundException($"No xml file present at path {Path.GetFullPath(sourceFileName)}");

            await using var sourceStream = File.Open(sourceFileName, FileMode.Open);
            var xDocument = await XDocument.LoadAsync(sourceStream, LoadOptions.None, CancellationToken.None);
            var root = xDocument.Root;

            if (root == null)
                throw new ArgumentException("XML document does not contain root node.");

            const string titleNodeName = "title";
            const string textNodeName = "text";

            var titleNode = root.Element(titleNodeName);

            if (titleNode == null)
                throw new ArgumentException($"Xml document does not contain required node with name {titleNodeName}");

            var textNode = root.Element(textNodeName);

            if (textNode == null)
                throw new ArgumentException($"Xml document does not contain required node with name {textNodeName}");

            // Is it required for title/text to be non null values ?
            var doc = new Document
            {
                Title = titleNode.Value,
                Text = textNode.Value
            };

            var jsonDocument = JsonConvert.SerializeObject(doc, Formatting.Indented);

            var directoryPath = Path.GetDirectoryName(targetFileName);

            if (directoryPath == null)
                throw new DirectoryNotFoundException($"Failed to get directory from file path {Path.GetFullPath(targetFileName)}");

            Directory.CreateDirectory(directoryPath);

            await File.WriteAllTextAsync(targetFileName, jsonDocument);
        }
    }
}
