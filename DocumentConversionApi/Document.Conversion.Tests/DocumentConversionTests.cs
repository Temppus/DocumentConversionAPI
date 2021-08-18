using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Document.Conversion.Tests
{
    public class DocumentConversionTests : IClassFixture<WebApplicationFactory<DocumentConversionApi.Startup>>
    {
        private const string ConvertUrl = "/Document/Convert";

        private readonly WebApplicationFactory<DocumentConversionApi.Startup> _factory;

        public DocumentConversionTests(WebApplicationFactory<DocumentConversionApi.Startup> factory)
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
            byteArrayContent.Headers.Remove("Content-Type");
            byteArrayContent.Headers.Add("Content-Type", "multipart/form-data; boundary=----WebKitFormBoundaryaYcxA3wAp5XMUV2w");

            multipartContent.Add(new StringContent("json"), "FileType");
            multipartContent.Add(byteArrayContent, "file", "test.json");
            
            var request = new HttpRequestMessage(HttpMethod.Post, ConvertUrl)
            {
                Content = multipartContent,
            };

            request.Headers.Add("accept", "application/xml");

            var response = await client.SendAsync(request);

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
            byteArrayContent.Headers.Remove("Content-Type");
            byteArrayContent.Headers.Add("Content-Type", "multipart/form-data; boundary=----WebKitFormBoundaryaYcxA3wAp5XMUV2w");

            multipartContent.Add(new StringContent("xml"), "FileType");
            multipartContent.Add(byteArrayContent, "file", "test.xml");

            var request = new HttpRequestMessage(HttpMethod.Post, ConvertUrl)
            {
                Content = multipartContent,
            };

            request.Headers.Add("accept", "application/json");

            var response = await client.SendAsync(request);
            
            response.EnsureSuccessStatusCode();

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsStringAsync();
            var responseDoc = JsonConvert.DeserializeObject<Document>(content);

            Assert.Equal("Sample title", responseDoc.Title);
            Assert.Equal("Very descriptive text ...", responseDoc.Text);
        }

        [Fact (Skip = "Prevent large uploads attacks")]
        public void Test_Large_Request_Size_Rejected() { }

        [Fact(Skip = "Assert bad request with validation message")]
        public void Test_Invalid_Accept_Header() { }

        [Fact(Skip = "Assert bad request when malformed file is uploaded for conversion")]
        public void Test_Malformed_Conversion_File() { }

        [Fact(Skip = "Assert bad request when some request form data fields are not present")]
        public void Test_Missing_File_Or_FileType() { }
    }
}
