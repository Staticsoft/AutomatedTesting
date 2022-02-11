using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Staticsoft.IntegrationTesting
{
    internal class ApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        readonly IServiceCollection ServiceCollection;

        public ApplicationFactory(IServiceCollection serviceCollection)
            => ServiceCollection = serviceCollection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
            => builder.UseStartup<TStartup>().ConfigureServices(services =>
            {
                foreach (var service in ServiceCollection)
                {
                    services.Add(service);
                };
            });
    }
}
