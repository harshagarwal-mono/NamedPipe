using H.Pipes;
using H.Pipes.Args;
using Newtonsoft.Json;

public class Program
{
    public static async Task Main(string[] args)
    {
        string pipeName = "poc-pipe";
        var server = new PipeServer<string>(pipeName);

        server.ClientConnected += async (o, args) =>
        {
            Console.WriteLine($"Client {args.Connection.PipeName} is now connected!");

            await args.Connection.WriteAsync(JsonConvert.SerializeObject(new Message("Welcome!")));
        };

        server.ClientDisconnected += (o, args) =>
        {
            Console.WriteLine($"Client {args.Connection.PipeName} disconnected");
        };

        server.MessageReceived += (sender, args) =>
        {
            Console.WriteLine($"Client {args.Connection.PipeName} says: {args.Message}");
        };

        server.ExceptionOccurred += (o, args) => OnExceptionOccurred(args.Exception);

        await server.StartAsync();

        await Task.Delay(Timeout.InfiniteTimeSpan);
    }

    private static void OnExceptionOccurred(Exception ex)
    {
        Console.WriteLine(ex);
    }
}

class Message
{
    public string Data;
    public string Method = "ResponseFakeMessage";

    public Message(string data)
    {
        Data = data;
    }
}
