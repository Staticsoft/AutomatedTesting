using Microsoft.Extensions.DependencyInjection;
using System;

namespace Staticsoft.Testing
{
    public class TestBase
    {
        readonly IServiceProvider Provider;

        public TestBase()
            => Provider = Services.BuildServiceProvider();

        virtual protected IServiceCollection Services
            => new ServiceCollection();

        protected T Get<T>()
            => Provider.GetRequiredService<T>();
    }

    public class TestBase<SystemUnderTest> : TestBase
        where SystemUnderTest : class
    {
        protected override IServiceCollection Services => base.Services
            .AddSingleton<SystemUnderTest>();

        protected SystemUnderTest SUT
            => Get<SystemUnderTest>();
    }
}
