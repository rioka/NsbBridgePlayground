using NsbBridgePlayground.Common.Attributes;

namespace NsbBridgePlayground.Common.Messages.Events;

[NsbEvent(Endpoints.OrderProcessor)]
public class OrderCreated
{
  public Guid Id { get; set; }
}