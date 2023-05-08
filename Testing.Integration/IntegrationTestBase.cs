using Microsoft.Extensions.DependencyInjection;
using System;

namespace Staticsoft.Testing.Integration;

public class IntegrationTestBase<Startup>
    where Startup : class
{
    readonly ApplicationFactory<Startup> Application;
    readonly IServiceProvider Provider;

    protected virtual IServiceCollection ServerServices(IServiceCollection services) => services;

    protected virtual IServiceCollection ClientServices(IServiceCollection services) => services
        .AddSingleton(_ => Application.Server.CreateClient())
        .AddSingleton(_ => Application.Server.CreateWebSocketClient());

    public IntegrationTestBase()
    {
        Application = new ApplicationFactory<Startup>(ServerServices);
        Provider = ClientServices(new ServiceCollection()).BuildServiceProvider();
    }

    protected Service Server<Service>()
        => Application.Server.Services.GetRequiredService<Service>();

    protected Service Client<Service>()
        => Provider.GetRequiredService<Service>();
}
