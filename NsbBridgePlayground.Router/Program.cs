using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace NsbBridgePlayground.Router;

internal class Program
{
  public static async Task Main(string[] args)
  {
    var host = CreateHostBuilder(args)
      .Build();

    await host.RunAsync();
  }

  private static IHostBuilder CreateHostBuilder(string[] args)
  {
    var hb = Host
      .CreateDefaultBuilder(args)
      .ConfigureAppConfiguration(builder => {

        builder.AddEnvironmentVariables("NSBROUTER_");
      })
      .UseConsoleLifetime();

    return hb;
  }
}