using System;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Converter.Sample
{
    internal static class OriginalSample
    {
        private class Document
        {
            public string Title { get; set; }
            public string Text { get; set; }
        }

        internal static void RunConversion()
        {
            var sourceFileName = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Source Files\\Document1.xml");
            var targetFileName = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Target Files\\Document1.json");

            string input;

            try
            {
                // TODO: missing using -> not disposing FileStream/StreamReader
                FileStream sourceStream = File.Open(sourceFileName, FileMode.Open);
                var reader = new StreamReader(sourceStream);

                // TODO: ideally use async overload
                input = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                // TODO: catching and rethrowing general exception is in general bad practice
                // TODO: + in this way we will lose stacktrace from original exception
                throw new Exception(ex.Message);
            }

            var xdoc = XDocument.Parse(input);

            // TODO: Possible null reference exceptions when accessing root/element nodes/values
            // TODO: Deserialization could be done via XMl annotations on model
            // TODO: I think this will not handle XML with namespaces correctly (not sure if it is requirement tyo handle this case)
            var doc = new Document
            {
                Title = xdoc.Root.Element("title").Value,
                Text = xdoc.Root.Element("text").Value
            };

            var serializedDoc = JsonConvert.SerializeObject(doc);

            // TODO: What if file path/file does not exists ?
            // TODO: What if file already exits ?
            var targetStream = File.Open(targetFileName, FileMode.Create, FileAccess.Write);
            var sw = new StreamWriter(targetStream);

            // TODO: ideally use async overload
            sw.Write(serializedDoc);
        }
    }
}
