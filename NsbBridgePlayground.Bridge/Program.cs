﻿using Microsoft.Extensions.Configuration;
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
      .ConfigureAppConfiguration(builder => {

        builder.AddEnvironmentVariables("NSBBRIDGE_");
      })
      .UseConsoleLifetime()
      .UseNServiceBusBridge((ctx, config) => {

        AddConfiguration(config, Endpoints.OrderProcessor, ctx.Configuration.GetConnectionString("OrderProcessor"));
        AddConfiguration(config, Endpoints.Shipping, ctx.Configuration.GetConnectionString("Shipping"), subscribedEvents: new [] {
          typeof(OrderCreated)
        });
        AddConfiguration(config, Endpoints.Notifier, ctx.Configuration.GetConnectionString("Notifier"), subscribedEvents: new [] {
          typeof(OrderCreated)
        });
        AddConfiguration(config, Endpoints.Sender, ctx.Configuration.GetConnectionString("Sender"));
        
        config.RunInReceiveOnlyTransactionMode();
      });

    return hb;
  }

  /// <summary>
  /// Instruct the bridge to look at queue for <paramref name="endpoint"/>.
  /// </summary>
  /// <param name="bridgeConfig">Configuration</param>
  /// <param name="endpoint">Name of the endpoint the bridge is configured for</param>
  /// <param name="connectionString">Connection string for <paramref name="endpoint"/></param>
  /// <param name="nsbSchema">Name of the schema (optional, default to <c>nsb</c>).</param>
  /// <param name="subscribedEvents">
  /// <para>
  ///   An optional list of events <paramref name="endpoint"/> has subscribed to.
  /// </para>
  /// <para>
  ///   For each type in this list, if that type is decorated with <see cref="NsbEventAttribute"/>,
  ///   the bridge will send a subscription request to <see cref="NsbEventAttribute.Publisher"/>.
  /// </para>
  /// </param>
  private static void AddConfiguration(
    BridgeConfiguration bridgeConfig,
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
      Name = $"SQL-{endpoint}",
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