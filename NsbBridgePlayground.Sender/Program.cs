using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
#if STANDALONE
using NsbBridgePlayground.StandAlone.Bootstrap;
using NsbBridgePlayground.StandAlone.Bootstrap.Infrastructure;
#else
using NsbBridgePlayground.Bootstrap;
using NsbBridgePlayground.Bootstrap.Infrastructure;
#endif
using NsbBridgePlayground.Common;
using NsbBridgePlayground.Common.Messages.Commands;
using NServiceBus;

#if STANDALONE
namespace NsbBridgePlayground.StandAlone.Sender;
#else
namespace NsbBridgePlayground.Sender;
#endif

internal partial class Program
{
#if STANDALONE
  private static readonly string ConnectionStringName = "NsbBridgePlayground";
#else
  private static readonly string ConnectionStringName = "Sender";
#endif
  
  public static async Task Main(string[] args)
  {
    var host = CreateHostBuilder(args)
      .Build();

    using (host)
    {
      var config = host.Services.GetRequiredService<IConfiguration>();
      await DbHelpers.EnsureDatabaseExists(config.GetConnectionString(ConnectionStringName));
      
      await host.StartAsync();
      var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
      var session = host.Services.GetRequiredService<IMessageSession>();
      var logger = host.Services.GetRequiredService<ILogger<Program>>();

      await SendMessages(session, logger); 

      lifetime.StopApplication();
      await host.WaitForShutdownAsync();
    }
  }

  private static IHostBuilder CreateHostBuilder(string[] args)
  {
    var hb = Host
      .CreateDefaultBuilder(args)
      .ConfigureAppConfiguration(builder => {

        builder.AddEnvironmentVariables("NSBBRIDGE_");
      })
      .ConfigureLogging(builder => {

        builder.AddSeq();
      })
      .UseConsoleLifetime()
      .UseNServiceBus(ctx => {

        var endpointConfig = Bootstrapper.Configure(Endpoints.Sender, ctx.Configuration.GetConnectionString(ConnectionStringName), messages: new [] {
          typeof(CreateOrder)
        });
        return endpointConfig;
      });
    
    return hb;
  }
}