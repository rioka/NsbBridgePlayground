using Microsoft.Extensions.Logging;
using NsbBridgePlayground.Shared.Messages.Commands;
using NsbBridgePlayground.Shared.Messages.Events;
using NServiceBus;

namespace NsbBridgePlayground.OrderProcessor.Handlers;

internal class CreateOrderHandler : IHandleMessages<CreateOrder>
{
  private readonly ILogger<CreateOrderHandler> _logger;

  public CreateOrderHandler(ILogger<CreateOrderHandler> logger)
  {
    _logger = logger;
  }
  
  /// <inheritdoc />
  public async Task Handle(CreateOrder message, IMessageHandlerContext context)
  {
    _logger.LogInformation("Processing {MessageType} for {OrderId}", nameof(CreateOrder), message.Id);

    await context.Publish(new OrderCreated() {
      Id = message.Id
    });
  }
}