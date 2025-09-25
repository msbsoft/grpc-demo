using Grpc.Net.Client;
using GrpcService1;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace GrpcService1.Tests.Integration;

public class GrpcIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly GrpcChannel _channel;
    private readonly Greeter.GreeterClient _client;

    public GrpcIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        var httpClient = _factory.CreateClient();

        _channel = GrpcChannel.ForAddress(httpClient.BaseAddress!, new GrpcChannelOptions
        {
            HttpClient = httpClient
        });

        _client = new Greeter.GreeterClient(_channel);
    }

    [Fact]
    public async Task SayHello_Integration_ReturnsExpectedGreeting()
    {
        // Arrange
        var request = new HelloRequest { Name = "Integration Test" };

        // Act
        var response = await _client.SayHelloAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("Hello Integration Test", response.Message);
    }

    [Fact]
    public async Task SayHello_WithEmptyName_Integration_ReturnsEmptyGreeting()
    {
        // Arrange
        var request = new HelloRequest { Name = "" };

        // Act
        var response = await _client.SayHelloAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("Hello ", response.Message);
    }

    [Theory]
    [InlineData("Alice")]
    [InlineData("Bob Smith")]
    [InlineData("José García")]
    [InlineData("Test User 123")]
    [InlineData("User@Domain.com")]
    public async Task SayHello_WithVariousNames_Integration_ReturnsCorrectGreeting(string name)
    {
        // Arrange
        var request = new HelloRequest { Name = name };

        // Act
        var response = await _client.SayHelloAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal($"Hello {name}", response.Message);
    }

    [Fact]
    public async Task SayHello_MultipleRequests_Integration_AllReturnCorrectly()
    {
        // Arrange
        var requests = new[]
        {
            new HelloRequest { Name = "User1" },
            new HelloRequest { Name = "User2" },
            new HelloRequest { Name = "User3" }
        };

        // Act
        var tasks = requests.Select(req => _client.SayHelloAsync(req).ResponseAsync);
        var responses = await Task.WhenAll(tasks);

        // Assert
        Assert.Equal(3, responses.Length);
        Assert.Equal("Hello User1", responses[0].Message);
        Assert.Equal("Hello User2", responses[1].Message);
        Assert.Equal("Hello User3", responses[2].Message);
    }

    [Fact]
    public async Task SayHello_ConcurrentRequests_Integration_AllReturnCorrectly()
    {
        // Arrange
        const int requestCount = 10;
        var tasks = new List<Task<HelloReply>>();

        // Act
        for (int i = 0; i < requestCount; i++)
        {
            var request = new HelloRequest { Name = $"User{i}" };
            tasks.Add(_client.SayHelloAsync(request).ResponseAsync);
        }

        var responses = await Task.WhenAll(tasks);

        // Assert
        Assert.Equal(requestCount, responses.Length);
        for (int i = 0; i < requestCount; i++)
        {
            Assert.Equal($"Hello User{i}", responses[i].Message);
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
    }
}