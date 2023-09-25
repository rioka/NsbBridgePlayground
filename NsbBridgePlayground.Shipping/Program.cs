using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#if STANDALONE
using NsbBridgePlayground.StandAlone.Bootstrap;
using NsbBridgePlayground.StandAlone.Bootstrap.Infrastructure;
#else
using NsbBridgePlayground.Bootstrap;
using NsbBridgePlayground.Bootstrap.Infrastructure;
#endif
using NsbBridgePlayground.Common;
using NServiceBus;

#if STANDALONE
namespace NsbBridgePlayground.StandAlone.Shipping;
#else
namespace NsbBridgePlayground.Shipping;
#endif

internal class Program 
{
#if STANDALONE
  private static readonly string ConnectionStringName = "NsbBridgePlayground";
#else
  private static readonly string ConnectionStringName = "Shipping";
#endif

  public static async Task Main(string[] args)
  {
    var host = CreateHostBuilder(args)
      .Build();

    var config = host.Services.GetRequiredService<IConfiguration>();
    await DbHelpers.EnsureDatabaseExists(config.GetConnectionString(ConnectionStringName));

    await host.RunAsync();
  }

  private static IHostBuilder CreateHostBuilder(string[] args)
  {
    var hb = Host
      .CreateDefaultBuilder(args)
      .ConfigureAppConfiguration(builder => {

        builder.AddEnvironmentVariables("NSBBRIDGE_");
      })
      .UseConsoleLifetime()
      .UseNServiceBus(ctx => {

        var endpointConfig = Bootstrapper.Configure(Endpoints.Shipping, ctx.Configuration.GetConnectionString(ConnectionStringName));
        return endpointConfig;
      });

    return hb;
  }
}