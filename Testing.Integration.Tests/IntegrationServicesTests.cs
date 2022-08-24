using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Staticsoft.TestServer;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Staticsoft.Testing.Integration.Tests
{
    public class IntegrationServicesTests : TestBase<IntegrationServices>
    {
        [Fact]
        public void RegisteresServicesOnlyOnce()
        {
            AssertRegisteredOnce<IServiceCollection>();
            AssertRegisteredOnce<TestService>();
        }

        [Fact]
        public async Task CanMakeHttpRequestToServer()
        {
            var testResponse = "TestResponse";
            SetHttpResponseBody(testResponse);
            var response = await MakeHttpRequest();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var message = await ReadResponseBody(response);
            Assert.Equal(testResponse, message);
        }

        [Fact]
        public async Task CanMakeWebsocketRequestToServer()
        {
            var client = Get<WebSocketClient>();
            var socket = await client.ConnectAsync(new Uri("ws://localhost/WebSocket"), CancellationToken.None);
            var receiveMessage = ReceiveMessage(socket);
            await SendWebSocketRequest(socket, "Ping");
            var response = await receiveMessage;
            Assert.Equal("Pong", response);
        }

        static async Task SendWebSocketRequest(WebSocket socket, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var buffer = new Memory<byte>(bytes);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        static async Task<string> ReceiveMessage(WebSocket socket)
        {
            var buffer = new Memory<byte>(new byte[32 * 1024]);
            await socket.ReceiveAsync(buffer, CancellationToken.None);
            var bytes = buffer.ToArray();
            return Encoding.UTF8.GetString(bytes).TrimEnd('\0');
        }

        void AssertRegisteredOnce<T>()
            => Assert.Single(Get<IServiceCollection>().Where(service => service.ServiceType == typeof(T)));

        void SetHttpResponseBody(string testResponse)
            => Get<TestService>().SetTestResponse(testResponse);

        Task<HttpResponseMessage> MakeHttpRequest()
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
