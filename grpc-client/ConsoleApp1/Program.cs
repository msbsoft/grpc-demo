// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using Grpc.Net.Client;
using ConsoleApp1;
using GrpcService1;

var orders = Enumerable.Range(1, 5)
  .Select(i => OrderBuilder.Create()
    .Id(i)
    .Price(10 * i)
    .ShippedTo(b =>
        b.Street($"{i} Main St")
        .City("Anytown")
        .State("CA")
        .Zip("12345"))
    .Build())
  .ToList();
//Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(orders, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

Order order = OrderBuilder.Create()
    .Id(1)
    .Price(100)
    .ShippedTo(b =>
        b.Street("123 Main St")
        .City("Anytown")
        .State("CA")
        .Zip("12345"))
    .Build();
//Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(order, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));


// Use Docker service name for containerized environment, fallback to localhost for development
string serverAddress = Environment.GetEnvironmentVariable("GRPC_SERVER_ADDRESS") ?? "http://localhost:5117";
using var channel = GrpcChannel.ForAddress(serverAddress);
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(
    new HelloRequest { Name = "GreeterClient" });
Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.Read();