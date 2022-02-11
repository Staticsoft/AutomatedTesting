using Microsoft.Extensions.DependencyInjection;
using Staticsoft.Testing;
using Staticsoft.TestServer;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Staticsoft.IntegrationTesting.Tests
{
    public class IntegrationServicesTests : TestBase<IntegrationServices>
    {
        [Fact]
        public async Task CanMakeRequestToServer()
        {
            var testResponse = "TestResponse";
            SetResponseBody(testResponse);
            var response = await MakeRequest();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var message = await ReadResponseBody(response);
            Assert.Equal(testResponse, message);
        }

        void SetResponseBody(string testResponse)
            => Get<TestService>().SetTestResponse(testResponse);

        Task<HttpResponseMessage> MakeRequest()
            => Get<HttpClient>().GetAsync("/TestRequest");

        static Task<string> ReadResponseBody(HttpResponseMessage response)
            => response.Content.ReadAsStringAsync();
    }

    public class IntegrationServices : IntegrationServicesBase<TestStartup>
    {
        protected override IServiceCollection Services => base.Services
            .AddSingleton<TestService, TestServiceMock>();
    }
}
