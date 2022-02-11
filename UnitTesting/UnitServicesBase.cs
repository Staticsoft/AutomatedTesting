using Microsoft.Extensions.DependencyInjection;
using System;

namespace Staticsoft.Testing
{
    public class UnitServicesBase : ServiceProviderFactory
    {
        protected virtual IServiceCollection Services
            => new ServiceCollection();

        public IServiceProvider Create()
            => Services.BuildServiceProvider();
    }
}
