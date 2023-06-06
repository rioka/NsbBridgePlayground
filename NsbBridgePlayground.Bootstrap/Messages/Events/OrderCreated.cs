using NsbBridgePlayground.Bootstrap.Attributes;
using NsbBridgePlayground.Common;

namespace NsbBridgePlayground.Bootstrap.Messages.Events;

[NsbEvent(Endpoints.OrderProcessor)]
public class OrderCreated
{
  public Guid Id { get; set; }
}