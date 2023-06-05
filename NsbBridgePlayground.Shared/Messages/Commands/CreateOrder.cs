using NsbBridgePlayground.Shared.Attributes;

namespace NsbBridgePlayground.Shared.Messages.Commands;

[NsbCommand(Endpoints.OrderProcessor)]
public class CreateOrder
{
  public Guid Id { get; set; }
}