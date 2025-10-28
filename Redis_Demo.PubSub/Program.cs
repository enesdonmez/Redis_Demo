using StackExchange.Redis;

ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("localhost:1453");
ISubscriber subscriber = connection.GetSubscriber();

while (true)
{
    Console.Write("Enter a message to publish (or 'exit' to quit):");
    string? message = Console.ReadLine();
    await subscriber.PublishAsync("demoChannel", message);
}