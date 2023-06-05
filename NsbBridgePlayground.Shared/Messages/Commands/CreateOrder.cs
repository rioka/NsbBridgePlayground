using NsbBridgePlayground.Shared.Attributes;

namespace NsbBridgePlayground.Shared.Messages.Commands;

[NsbCommand(Endpoints.Sender)]
public class CreateOrder
{
  public Guid Id { get; set; }
}