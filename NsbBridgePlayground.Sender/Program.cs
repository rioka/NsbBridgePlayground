﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NsbBridgePlayground.Bootstrap.Infrastructure;
using NsbBridgePlayground.Bootstrap.Messages.Commands;
using NsbBridgePlayground.Common;
using NServiceBus;

namespace NsbBridgePlayground.Sender;

internal partial class Program
{
  public static async Task Main(string[] args)
  {
    var host = CreateHostBuilder(args)
      .Build();

    using (host)
    {
      var config = host.Services.GetRequiredService<IConfiguration>();
      await DbHelpers.EnsureDatabaseExists(config.GetConnectionString("Data"));
      
      await host.StartAsync();
      var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
      var session = host.Services.GetRequiredService<IMessageSession>();

      await SendMessages(session); 

      lifetime.StopApplication();
      await host.WaitForShutdownAsync();
    }
  }

  private static IHostBuilder CreateHostBuilder(string[] args)
  {
    var hb = Host
      .CreateDefaultBuilder(args)
      .UseConsoleLifetime()
      .UseNServiceBus(ctx => {

        var endpointConfig = Bootstrapper.Configure(Endpoints.Sender, ctx.Configuration.GetConnectionString("Data"), messages: new [] {
          typeof(CreateOrder)
        });
        return endpointConfig;
      });
    
    return hb;
  }
}