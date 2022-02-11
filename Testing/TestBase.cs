using Microsoft.Extensions.DependencyInjection;
using System;

namespace Staticsoft.Testing
{
    public abstract class TestBase<TSPF>
        where TSPF : ServiceProviderFactory, new()
    {
        readonly IServiceProvider Provider;

        public TestBase()
            => Provider = new TSPF().Create();

        protected T Get<T>()
            => Provider.GetRequiredService<T>();
    }

    public abstract class TestBase<TSUT, TSPF> : TestBase<TSPF>
        where TSUT : class
        where TSPF : ServiceProviderFactory, new()
    {
        protected readonly TSUT SUT;

        public TestBase()
            => SUT = Get<TSUT>();
    }
}
