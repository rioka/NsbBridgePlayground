using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NsbBridgePlayground.Bootstrap;
using NsbBridgePlayground.Bootstrap.Infrastructure;
using NsbBridgePlayground.Common;
using NServiceBus;

namespace NsbBridgePlayground.Notifier;

internal class Program
{
  public static async Task Main(string[] args)
  {
    var host = CreateHostBuilder(args)
      .Build();

    var config = host.Services.GetRequiredService<IConfiguration>();
    await DbHelpers.EnsureDatabaseExists(config.GetConnectionString("Notifier"));

    await host.RunAsync();
  }

  private static IHostBuilder CreateHostBuilder(string[] args)
  {
    var hb = Host
      .CreateDefaultBuilder(args)
      .ConfigureHostConfiguration(builder => {

        builder.AddEnvironmentVariables("NSBBRIDGE_");
      })
      .UseConsoleLifetime()
      .UseNServiceBus(ctx => {

        var endpointConfig = Bootstrapper.Configure(Endpoints.Notifier, ctx.Configuration.GetConnectionString("Notifier"));
        return endpointConfig;
      });

    return hb;
  }
}