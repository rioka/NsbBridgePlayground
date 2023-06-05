﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NsbBridgePlayground.Shared;
using NsbBridgePlayground.Shared.Infrastructure;
using NServiceBus;

internal class Program {
  public static async Task Main(string[] args)
  {
    var host = CreateHostBuilder(args)
      .Build();

    var config = host.Services.GetRequiredService<IConfiguration>();
    await DbHelpers.EnsureDatabaseExists(config.GetConnectionString("Data"));
    
    await host.RunAsync();
  }

  private static IHostBuilder CreateHostBuilder(string[] args)
  {
    var hb = Host
      .CreateDefaultBuilder()
      .UseConsoleLifetime()
      .UseNServiceBus(ctx =>  {

        var endpointConfig = Bootstrapper.Configure(Endpoints.OrderProcessor, ctx.Configuration.GetConnectionString("Data"));
        return endpointConfig;
      });
    
    return hb;
  }
}