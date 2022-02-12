using Microsoft.Extensions.DependencyInjection;
using Staticsoft.Testing.Integration;
using System;

namespace Staticsoft.Testing
{
    public abstract class IntegrationServicesBase<TStartup> : ServiceProviderFactory
        where TStartup : class
    {
        readonly ApplicationFactory<TStartup> Factory;

        public IntegrationServicesBase()
            => Factory = new ApplicationFactory<TStartup>(Services);

        public IServiceProvider Create()
            => Factory.Services;

        protected virtual IServiceCollection Services => new ServiceCollection()
            .AddSingleton(_ => Factory.CreateClient());
    }
}
