using NsbBridgePlayground.Bootstrap.Attributes;
using NsbBridgePlayground.Common;

namespace NsbBridgePlayground.Bootstrap.Messages.Commands;

[NsbCommand(Endpoints.OrderProcessor)]
public class CreateOrder
{
  public Guid Id { get; set; }
}