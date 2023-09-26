using Microsoft.Extensions.Logging;
using NsbBridgePlayground.Common.Messages.Events;
using NServiceBus;

#if STANDALONE
namespace NsbBridgePlayground.StandAlone.Shipping.Handlers;
#else
namespace NsbBridgePlayground.Shipping.Handlers;
#endif

internal class OrderCreatedHandler : IHandleMessages<OrderCreated>
{
  private readonly ILogger<OrderCreatedHandler> _logger;

  public OrderCreatedHandler(ILogger<OrderCreatedHandler> logger)
  {
    _logger = logger;
  }
  
  /// <inheritdoc />
  public Task Handle(OrderCreated message, IMessageHandlerContext context)
  {
    _logger.LogInformation("Processing {MessageType} for {OrderId}", nameof(OrderCreated), message.Id);
    return Task.CompletedTask;
  }
}