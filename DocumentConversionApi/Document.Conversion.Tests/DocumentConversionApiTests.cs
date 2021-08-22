using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Document.Conversion.DocumentStorage;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Document.Conversion.Tests
{
    public class DocumentConversionApiTests : IClassFixture<WebApplicationFactory<DocumentConversionApi.Startup>>
    {
        private const string ConvertUrl = "/Document/Convert";

        private readonly WebApplicationFactory<DocumentConversionApi.Startup> _factory;

        public DocumentConversionApiTests(WebApplicationFactory<DocumentConversionApi.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Test_Json_To_Xml_Conversion()
        {
            var options = new WebApplicationFactoryClientOptions();
            using var client = _factory.CreateClient(options);

            var doc = new Document
            {
                Title = "dummy title",
                Text = "text 123"
            };

            string json = JsonConvert.SerializeObject(doc);

            using var multipartContent = new MultipartFormDataContent();

            using var byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes(json));

            multipartContent.Add(new StringContent("json"), "InputFileType");
            multipartContent.Add(new StringContent("xml"), "OutputFileType");

            multipartContent.Add(new StringContent("file"), "OutputFileProvider");
            multipartContent.Add(byteArrayContent, "file", "test.json");

            var request = new HttpRequestMessage(HttpMethod.Post, ConvertUrl)
            {
                Content = multipartContent,
            };

            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            Assert.Equal("application/xml", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsStringAsync();
            var responseDoc = XDocument.Parse(content);

            Assert.Equal(doc.Title, responseDoc.Root.Element(nameof(Document.Title)).Value);
            Assert.Equal(doc.Text, responseDoc.Root.Element(nameof(Document.Text)).Value);
        }

        [Fact]
        public async Task Test_Json_File_Path_To_Xml_Conversion()
        {
            var options = new WebApplicationFactoryClientOptions();
            using var client = _factory.CreateClient(options);

            var doc = new Document
            {
                Title = "dummy temp file title",
                Text = "text 123"
            };

            const string fileName = "test.json";
            var commonAppDataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData, Environment.SpecialFolderOption.Create);
            var fullJsonFilePath = Path.Combine(commonAppDataDirPath, FileDocumentStorage.FileStorageDirName);
            await File.WriteAllTextAsync(fullJsonFilePath, JsonConvert.SerializeObject(doc));

            using var multipartContent = new MultipartFormDataContent
            {
                {new StringContent("json"), "InputFileType"},
                {new StringContent("xml"), "OutputFileType"},

                {new StringContent("file"), "InputFileProvider"},
                {new StringContent(fileName), "FileSource"},

                {new StringContent("file"), "OutputFileProvider"}
            };

            var request = new HttpRequestMessage(HttpMethod.Post, ConvertUrl)
            {
                Content = multipartContent,
            };

            var response = await client.SendAsync(request);

            File.Delete(fullJsonFilePath);

            response.EnsureSuccessStatusCode();

            Assert.Equal("application/xml", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsStringAsync();
            var responseDoc = XDocument.Parse(content);

            Assert.Equal(doc.Title, responseDoc.Root.Element(nameof(Document.Title)).Value);
            Assert.Equal(doc.Text, responseDoc.Root.Element(nameof(Document.Text)).Value);
        }

        [Fact]
        public async Task Test_Xml_To_Json_Conversion()
        {
            var options = new WebApplicationFactoryClientOptions();
            using var client = _factory.CreateClient(options);

            const string xml = "<?xml version=\"1.0\"?>\r\n<document>\r\n    <title>Sample title</title>\r\n    <text>Very descriptive text ...</text>\r\n</document>";

            using var multipartContent = new MultipartFormDataContent();
            
            using var byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes(xml));

            multipartContent.Add(new StringContent("xml"), "InputFileType");
            multipartContent.Add(new StringContent("json"), "OutputFileType");

            multipartContent.Add(new StringContent("file"), "OutputFileProvider");
            multipartContent.Add(byteArrayContent, "file", "test.xml");

            var request = new HttpRequestMessage(HttpMethod.Post, ConvertUrl)
            {
                Content = multipartContent,
            };

            var response = await client.SendAsync(request);
            
            response.EnsureSuccessStatusCode();

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsStringAsync();
            var responseDoc = JsonConvert.DeserializeObject<Document>(content);

            Assert.Equal("Sample title", responseDoc.Title);
            Assert.Equal("Very descriptive text ...", responseDoc.Text);
        }

        [Theory(Skip = "Prevent directory traversal attacks")]
        [InlineData("some full path")]
        [InlineData("relative path with ..\\ ../")]
        public void Test_Directory_Traversal_File_Attacks(string filePath) { }

        [Fact (Skip = "Prevent large uploads attacks")]
        public void Test_Large_Request_File_Size_Rejected() { }

        [Fact(Skip = "Prevent large download attacks")]
        public void Test_Large_Http_File_Download_Rejected() { }

        [Fact(Skip = "Assert bad request when malformed file is uploaded for conversion")]
        public void Test_Malformed_Conversion_File() { }

        [Fact(Skip = "Assert bad request when some mandatory request form data fields are not present")]
        public void Test_Missing_File_Or_FileType() { }
    }
}
