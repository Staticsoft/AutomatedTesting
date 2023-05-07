using Microsoft.Extensions.DependencyInjection;
using Staticsoft.TestServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Staticsoft.Testing.Integration.Tests
{
    public class IntegrationServicesTests : IntegrationTestBase<TestStartup>
    {
        protected override IServiceCollection ServerServices(IServiceCollection services) => base.ServerServices(services)
            .AddSingleton<TestService, TestServiceMock>();

        [Fact]
        public void RegisteresServicesOnlyOnce()
        {
            AssertRegisteredOnce<IServiceCollection>();
            AssertRegisteredOnce<TestService>();
        }

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

        void AssertRegisteredOnce<T>()
            => Assert.Single(Server<IServiceCollection>().Where(service => service.ServiceType == typeof(T)));

        void SetResponseBody(string testResponse)
            => Server<TestService>().SetTestResponse(testResponse);

        Task<HttpResponseMessage> MakeRequest()
            => Client<HttpClient>().GetAsync("/TestRequest");

        static Task<string> ReadResponseBody(HttpResponseMessage response)
            => response.Content.ReadAsStringAsync();
    }
}
