using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Staticsoft.TestServer
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services) => services
            .AddSingleton(services)
            .AddControllers();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment _) => app
            .UseRouting()
            .UseEndpoints(endpoints => endpoints.MapControllers())
            .UseWebSockets()
            .Use(HandleWebSocketRequests);

        static async Task HandleWebSocketRequests(HttpContext context, Func<Task> next)
        {
            if (context.WebSockets.IsWebSocketRequest && context.Request.Path == "/WebSocket")
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                await ReceiveMessages(socket);
            }
            else
            {
                await next();
            }
        }

        static async Task ReceiveMessages(WebSocket socket)
        {
            while (socket.State == WebSocketState.Open)
            {
                var buffer = new Memory<byte>(new byte[32 * 1024]);
                await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (socket.State == WebSocketState.CloseReceived)
                {
                    socket.Dispose();
                }
                else
                {
                    var bytes = buffer.ToArray();
                    var message = Encoding.UTF8.GetString(bytes).TrimEnd('\0');
                    if (message == "Ping")
                    {
                        await SendPong(socket);
                    }
                }
            }
        }

        static Task SendPong(WebSocket socket)
            => SendWebSocketRequest(socket, "Pong");

        static async Task SendWebSocketRequest(WebSocket socket, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var buffer = new Memory<byte>(bytes);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
