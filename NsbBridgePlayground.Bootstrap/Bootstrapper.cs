using Microsoft.Data.SqlClient;
using NsbBridgePlayground.Common.Attributes;
using NServiceBus;
using System.Reflection;

namespace NsbBridgePlayground.Bootstrap;

public class Bootstrapper
{
  public static Task<IEndpointInstance> Start(string endpointName, string connectionString) 
  {
    return Endpoint.Start(Configure(endpointName, connectionString));
  }

  public static EndpointConfiguration Configure(
    string endpointName, 
    string connectionString, 
    string? nsbSchema = "nsb", 
    IEnumerable<Type>? messages = null)
  {
    var config = new EndpointConfiguration(endpointName);

    ConfigureConventions(config);
    ConfigureTransport(config, connectionString, nsbSchema, messages);
    ConfigurePersistence(config, connectionString, nsbSchema);

    config.AuditProcessedMessagesTo("audit");
    config.SendFailedMessagesTo("error");
    
    config.EnableInstallers();

    return config;
  }

  #region Internals

  private static void ConfigureConventions(EndpointConfiguration config)
  {
    config.Conventions().DefiningCommandsAs(t => t.Namespace?.Contains("Messages.Commands") ?? false);
    config.Conventions().DefiningEventsAs(t => t.Namespace?.Contains("Messages.Events") ?? false);
  }

  private static void ConfigureTransport(
    EndpointConfiguration config, 
    string connectionString,
    string? nsbSchema = null,
    IEnumerable<Type>? messages = null)
  {
    var transport = config.UseTransport<SqlServerTransport>();
    transport
      .Transactions(TransportTransactionMode.TransactionScope)
      .ConnectionString(connectionString);

    if (!string.IsNullOrWhiteSpace(nsbSchema))
    {
      transport.DefaultSchema(nsbSchema);
    }

    ConfigureRoutes(transport.Routing(), messages ?? Enumerable.Empty<Type>());
  }

  private static void ConfigureRoutes(RoutingSettings<SqlServerTransport> routing, IEnumerable<Type> messages)
  {
    foreach (var type in messages)
    {
      var routingInfo = type.GetCustomAttribute<NsbCommandAttribute>() as Attribute ?? type.GetCustomAttribute<NsbEventAttribute>();
      if (routingInfo is null)
      {
        continue;
      }

      switch (routingInfo)
      {
        case NsbCommandAttribute command:
          routing.RouteToEndpoint(type, command.Recipient);
          break;
        
        case NsbEventAttribute @event:
          // native subscription, nothing to do so far
          break;
        
        default:
          throw new NotSupportedException($"Unsupported attribute {routingInfo.GetType()} for {type}");
      }
    }
  }

  private static void ConfigurePersistence(EndpointConfiguration config, string connectionString, string? nsbSchema = null)
  {
    var persistence = config.UsePersistence<SqlPersistence>();
    persistence
      .ConnectionBuilder(() => new SqlConnection(connectionString));
    var sqlSettings = persistence
      .SqlDialect<SqlDialect.MsSqlServer>();
    
    if (!string.IsNullOrWhiteSpace(nsbSchema))
    {
      sqlSettings.Schema(nsbSchema);
    }
  }

  #endregion    
}