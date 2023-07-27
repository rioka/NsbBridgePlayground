using Microsoft.Extensions.Logging;
using NsbBridgePlayground.Common.Messages;
using NServiceBus;

namespace NsbBridgePlayground.Sender.Handlers;

internal class CreateOrderResponseHandler : IHandleMessages<CreateOrderResponse>
{
  private readonly ILogger<CreateOrderResponseHandler> _logger;

  public CreateOrderResponseHandler(ILogger<CreateOrderResponseHandler> logger)
  {
    _logger = logger;
  }
  
  public Task Handle(CreateOrderResponse message, IMessageHandlerContext context)
  {
    _logger.LogInformation("Processing {MessageType} for {OrderId}", nameof(CreateOrderResponse), message.Id);
    _logger.LogInformation("Order {Id} was created - {Notes}", message.Id, message.Notes ?? "<No notes found>");
    
    return Task.CompletedTask;
  }
}