using NsbBridgePlayground.Common.Messages.Commands;
using NServiceBus;
using System.Runtime.CompilerServices;

namespace NsbBridgePlayground.Sender;

internal partial class Program
{
  private static async Task SendMessages(IMessageSession session)
  {
    var exit = false;
    
    while (!exit)
    {
      Console.WriteLine("Press '1' enter to create an order");
      Console.WriteLine("Press '2' enter to create 10 orders");
      Console.WriteLine("Press '3' enter to create 50 orders");
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
          try
          {
            await session.Send(createOrder);
          }
          catch (Exception e)
          {
            Console.WriteLine(e.Message);
          }
          break;

        case ConsoleKey.D2:
        case ConsoleKey.NumPad2:
          await BatchCreate(session, 10);
          break;

        case ConsoleKey.D3:
        case ConsoleKey.NumPad3:
          await BatchCreate(session, 50);
          break;

        default:
          exit = true;
          break;
      }
    }
  }

  private static async Task BatchCreate(IMessageSession session, int count)
  {
    Console.WriteLine($"Sending {count} message(s)...");

    Task? all = default; 

    try
    {
      var tasks = new Task[count];
      for (int i = 0; i < count; i++)
      {
        var createOrder = new CreateOrder() {
          Id = Guid.NewGuid()
        };
        // we can get an exception here, eg before tasks are awaited
        // under heavy load, we may experience timeouts when trying to connect to the server
        tasks[i] = session.Send(createOrder);
      }

      all = Task.WhenAll(tasks);
      await all;
    }
    catch (Exception e)
    {
      var failed = all?.Exception?.InnerExceptions.Count ?? 0;
      Console.WriteLine($"{failed} 'Send' operation(s) failed");
      if (failed > 0)
      {
        // failed while awaiting
        foreach (var ie in all!.Exception!.InnerExceptions)
        {
          Console.WriteLine(ie.Message);
        }     
      }
      else
      {
        // failed before awaiting (TBC)
        Console.WriteLine(e.Message);
      }
    }
  }
}