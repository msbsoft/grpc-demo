using Grpc.Core;
using GrpcService1.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GrpcService1.Tests.Services;

public class GreeterServiceTests
{
    private readonly Mock<ILogger<GreeterService>> _mockLogger;
    private readonly GreeterService _service;

    public GreeterServiceTests()
    {
        _mockLogger = new Mock<ILogger<GreeterService>>();
        _service = new GreeterService(_mockLogger.Object);
    }

    [Fact]
    public async Task SayHello_WithValidRequest_ReturnsGreeting()
    {
        // Arrange
        var request = new HelloRequest { Name = "World" };
        var context = new Mock<ServerCallContext>().Object;

        // Act
        var result = await _service.SayHello(request, context);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Hello World", result.Message);
    }

    [Fact]
    public async Task SayHello_WithEmptyName_ReturnsGreetingWithEmptyName()
    {
        // Arrange
        var request = new HelloRequest { Name = "" };
        var context = new Mock<ServerCallContext>().Object;

        // Act
        var result = await _service.SayHello(request, context);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Hello ", result.Message);
    }

    [Fact]
    public async Task SayHello_WithSpecialCharacters_ReturnsGreetingWithSpecialCharacters()
    {
        // Arrange
        var request = new HelloRequest { Name = "John@#$%^&*()" };
        var context = new Mock<ServerCallContext>().Object;

        // Act
        var result = await _service.SayHello(request, context);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Hello John@#$%^&*()", result.Message);
    }

    [Fact]
    public async Task SayHello_WithLongName_ReturnsGreetingWithLongName()
    {
        // Arrange
        var longName = new string('A', 1000);
        var request = new HelloRequest { Name = longName };
        var context = new Mock<ServerCallContext>().Object;

        // Act
        var result = await _service.SayHello(request, context);

        // Assert
        Assert.NotNull(result);
        Assert.Equal($"Hello {longName}", result.Message);
    }

    [Fact]
    public async Task SayHello_WithUnicodeCharacters_ReturnsGreetingWithUnicodeCharacters()
    {
        // Arrange
        var request = new HelloRequest { Name = "José García 🌟" };
        var context = new Mock<ServerCallContext>().Object;

        // Act
        var result = await _service.SayHello(request, context);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Hello José García 🌟", result.Message);
    }

    [Theory]
    [InlineData("Alice")]
    [InlineData("Bob")]
    [InlineData("Charlie")]
    [InlineData("123")]
    [InlineData("Test User")]
    public async Task SayHello_WithVariousNames_ReturnsCorrectGreeting(string name)
    {
        // Arrange
        var request = new HelloRequest { Name = name };
        var context = new Mock<ServerCallContext>().Object;

        // Act
        var result = await _service.SayHello(request, context);

        // Assert
        Assert.NotNull(result);
        Assert.Equal($"Hello {name}", result.Message);
    }
}