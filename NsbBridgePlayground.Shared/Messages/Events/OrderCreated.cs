using NsbBridgePlayground.Shared.Attributes;

namespace NsbBridgePlayground.Shared.Messages.Events;

[NsbEvent(Endpoints.OrderProcessor)]
public class OrderCreated
{
  public Guid Id { get; set; }
}