using Dapper;
using Microsoft.Extensions.Logging;
using NsbBridgePlayground.Common.Messages;
using NsbBridgePlayground.Common.Messages.Commands;
using NsbBridgePlayground.Common.Messages.Events;
using NServiceBus;
using NServiceBus.Persistence;

namespace NsbBridgePlayground.OrderProcessor.Handlers;

internal class CreateOrderHandler : IHandleMessages<CreateOrder>
{
  private static readonly string INSERT_ORDER = @"
INSERT INTO [dbo].[Orders] (
  [UId], [Notes], [CreatedAt]
)
OUTPUT inserted.id
VALUES (
  @uid, @notes, @createdAt
)
";
  private readonly ILogger<CreateOrderHandler> _logger;

  public CreateOrderHandler(ILogger<CreateOrderHandler> logger)
  {
    _logger = logger;
  }
  
  /// <inheritdoc />
  public async Task Handle(CreateOrder message, IMessageHandlerContext context)
  {
    _logger.LogInformation("Processing {MessageType} for {OrderId}", nameof(CreateOrder), message.Id);

    var orderId = await RegisterOrder(message, context.SynchronizedStorageSession);
    
    await context.Publish(new OrderCreated() {
      Id = message.Id
    });
    await context.Reply(new CreateOrderResponse() {
      Id = message.Id,
      Notes = $"Order created at {DateTime.UtcNow}"
    });
  }

  private async Task<int> RegisterOrder(CreateOrder order, SynchronizedStorageSession storageSession)
  {
    var cn = storageSession.SqlPersistenceSession().Connection;
    var id = await cn.ExecuteScalarAsync<int>(INSERT_ORDER, new {
      uid = order.Id,
      notes = order.Notes,
      createdAt = DateTime.UtcNow
    });

    return id;
  }
}