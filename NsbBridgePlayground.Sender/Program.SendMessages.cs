﻿using NsbBridgePlayground.Shared;
using NsbBridgePlayground.Shared.Messages.Commands;
using NServiceBus;

internal partial class Program
{
  private static async Task SendMessages(IMessageSession session)
  {
    var exit = false;
    
    while (!exit)
    {
      Console.WriteLine("Press '1' enter to create an order");
      Console.WriteLine("Press any key to exit");
      
      var ch = Console.ReadKey();
      Console.WriteLine();

      switch (ch.Key)
      {
        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:
          var createOrder = new CreateOrder() {
            Id = Guid.NewGuid()
          };
          await session.Send(Endpoints.OrderProcessor, createOrder);
          break;

        default:
          exit = true;
          break;
      }
    }
  }
}