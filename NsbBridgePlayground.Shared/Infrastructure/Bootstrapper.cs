using Microsoft.Data.SqlClient;
using NServiceBus;

namespace NsbBridgePlayground.Shared.Infrastructure;

public class Bootstrapper
{
  public static Task<IEndpointInstance> Start(string endpointName, string connectionString) 
  {
    return Endpoint.Start(Configure(endpointName, connectionString));
  }

  public static EndpointConfiguration Configure(string endpointName, string connectionString, string? nsbSchema = "nsb")
  {
    var config = new EndpointConfiguration(endpointName);

    ConfigureRouting(config);
    ConfigureTransport(config, connectionString, nsbSchema);
    ConfigurePersistence(config, connectionString, nsbSchema);

    config.AuditProcessedMessagesTo("audit");
    config.SendFailedMessagesTo("error");
    
    config.EnableInstallers();

    return config;
  }

  #region Internals

  private static void ConfigureRouting(EndpointConfiguration config)
  {
    config.Conventions().DefiningCommandsAs(t => t.Namespace?.Contains("Messages.Commands") ?? false);
    config.Conventions().DefiningEventsAs(t => t.Namespace?.Contains("Messages.Events") ?? false);
  }

  private static void ConfigureTransport(EndpointConfiguration config, string connectionString, string? nsbSchema = null)
  {
    var transport = config.UseTransport<SqlServerTransport>();
    transport
      .Transactions(TransportTransactionMode.TransactionScope)
      .ConnectionString(connectionString);

    if (!string.IsNullOrWhiteSpace(nsbSchema))
    {
      transport.DefaultSchema(nsbSchema);
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