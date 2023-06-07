using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NsbBridgePlayground.Common;
using NsbBridgePlayground.Common.Attributes;
using NsbBridgePlayground.Common.Messages.Events;
using System.Reflection;

namespace NsbBridgePlayground.Bridge;

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
      .UseConsoleLifetime()
      .UseNServiceBusBridge((ctx, config) => {

        AddConfiguration(config, Endpoints.Sender, ctx.Configuration.GetConnectionString("Sender"));
        AddConfiguration(config, Endpoints.OrderProcessor, ctx.Configuration.GetConnectionString("OrderProcessor"));
        AddConfiguration(config, Endpoints.Notifier, ctx.Configuration.GetConnectionString("Notifier"), subscribedEvents: new [] {
          typeof(OrderCreated)
        });
        AddConfiguration(config, Endpoints.Shipping, ctx.Configuration.GetConnectionString("Shipping"), subscribedEvents: new [] {
          typeof(OrderCreated)
        });
        
        config.RunInReceiveOnlyTransactionMode();
      });

    return hb;
  }

  private static void AddConfiguration(BridgeConfiguration bridgeConfig,
    string endpoint,
    string? connectionString,
    string? nsbSchema = "nsb",
    IEnumerable<Type>? subscribedEvents = null)
  {
    if (string.IsNullOrWhiteSpace(connectionString))
    {
      // log warning
      return;
    }

    var sqlTransport = new SqlServerTransport(connectionString);
    if (!string.IsNullOrWhiteSpace(nsbSchema))
    {
      sqlTransport.DefaultSchema = nsbSchema;
    }
    
    var bridgeTransport = new BridgeTransport(sqlTransport) {
      // all transports are SqlServerTransport, so we give each a name to be able to identify the proper one in logs
      Name = $"For{endpoint}",
      AutoCreateQueues = true
    };

    var bridgeEndpoint = new BridgeEndpoint(endpoint);

    foreach (var type in subscribedEvents ?? Enumerable.Empty<Type>())
    {
      var routingInfo = type.GetCustomAttribute<NsbEventAttribute>();
      if (routingInfo != null)
      {
        bridgeEndpoint.RegisterPublisher(type, routingInfo.Publisher);
      }
      // else
      // {
      //   // throw?
      // }
    }
    
    bridgeTransport.HasEndpoint(bridgeEndpoint);
    bridgeConfig.AddTransport(bridgeTransport);
  }
}