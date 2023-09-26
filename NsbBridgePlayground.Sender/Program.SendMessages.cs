using Microsoft.Extensions.Logging;
using NsbBridgePlayground.Common.Messages.Commands;
using NServiceBus;

#if STANDALONE
namespace NsbBridgePlayground.StandAlone.Sender;
#else
namespace NsbBridgePlayground.Sender;
#endif

internal partial class Program
{
  private static async Task SendMessages(IMessageSession session, ILogger<Program> logger)
  {
    var exit = false;
    
    while (!exit)
    {
      Console.WriteLine("Press '1' enter to create an order");
      Console.WriteLine("Press '2' enter to create 10 orders");
      Console.WriteLine("Press '3' enter to create 50 orders");
      Console.WriteLine("Press '4' enter to create an order with large payload");
      Console.WriteLine("Press '5' enter to create 10 orders with large payload");
      Console.WriteLine("Press '6' enter to create 50 orders with large payload");
      Console.WriteLine("Press any key to exit");
      
      var ch = Console.ReadKey();
      Console.WriteLine();

      switch (ch.Key)
      {
        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:
        case ConsoleKey.D4:
        case ConsoleKey.NumPad4:
          var createOrder = new CreateOrder() {
            Id = Guid.NewGuid()
          };
          if (ch.Key is ConsoleKey.D4 or ConsoleKey.NumPad4)
          {
            SetNotes(createOrder);
          }
          try
          {
            await session.Send(createOrder);
          }
          catch (Exception e)
          {
            logger.LogError(e, "Failed to send '{Command}'", nameof(CreateOrder));
          }
          break;

        case ConsoleKey.D2:
        case ConsoleKey.NumPad2:
        case ConsoleKey.D5:
        case ConsoleKey.NumPad5:
          await BatchCreate(session, 10, ch.Key is ConsoleKey.D5 or ConsoleKey.NumPad5, logger);
          break;

        case ConsoleKey.D3:
        case ConsoleKey.NumPad3:
        case ConsoleKey.D6:
        case ConsoleKey.NumPad6:
          await BatchCreate(session, 50, ch.Key is ConsoleKey.D6 or ConsoleKey.NumPad6, logger);
          break;

        default:
          exit = true;
          break;
      }
    }
  }

  private static void SetNotes(CreateOrder command)
  {
    // 10-11 words
    // 4-5 sentences
    command.Notes = string.Join(Environment.NewLine, LoremNET.Lorem.Paragraphs(10, 11, 4, 5, 5, 15));
  }

  private static async Task BatchCreate(IMessageSession session, int count, bool setNotes, ILogger<Program> logger)
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
      logger.LogWarning("{ErrorCount} 'Send' operation(s) failed", failed);
      if (failed > 0)
      {
        // failed while awaiting
        foreach (var ie in all!.Exception!.InnerExceptions)
        {
          logger.LogError(ie, "Failed request while sending {Count} commands", count);
        }     
      }
      else
      {
        // failed before awaiting (TBC)
        logger.LogError(e, "Failure before awaiting (TBC) while sending {Count} commands", count);
      }
    }
  }
}