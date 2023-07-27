using NsbBridgePlayground.Common.Attributes;

namespace NsbBridgePlayground.Common.Messages.Commands;

[NsbCommand(Endpoints.OrderProcessor)]
public class CreateOrder
{
  public Guid Id { get; set; }

  public string Notes { get; set; }
}