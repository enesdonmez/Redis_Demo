using StackExchange.Redis;

ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("localhost:1453");
ISubscriber subscriber = connection.GetSubscriber();

await subscriber.SubscribeAsync("demoChannel", (channel, message) =>
{
    Console.WriteLine($"Received message: {message}");
});

Console.Read();