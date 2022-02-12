using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Staticsoft.Testing.Unit.Tests
{
    public class TestBaseTests : TestBase<SUT, SUTDependencies>
    {
        [Fact]
        public void CanGetDependency()
        {
            Assert.Equal(typeof(Dependency), Get<Dependency>().GetType());
        }

        [Fact]
        public void CanGetSUT()
        {
            Assert.Equal(typeof(SUT), SUT.GetType());
        }

        [Fact]
        public void CanUseDependency()
        {
            Assert.Equal(42, SUT.GetMagicNumber());
        }
    }

    public class SUTDependencies : UnitServicesBase
    {
        protected override IServiceCollection Services => base.Services
            .AddSingleton<Dependency>()
            .AddSingleton<SUT>();
    }

    public class SUT
    {
        readonly Dependency Dependency;

        public SUT(Dependency dependency)
            => Dependency = dependency;

        public int GetMagicNumber()
            => Dependency.GetMagicNumber();
    }

    public class Dependency
    {
        public int GetMagicNumber()
            => 42;
    }
}
