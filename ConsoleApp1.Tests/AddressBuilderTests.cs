using ConsoleApp1;
using Xunit;

namespace ConsoleApp1.Tests;

public class AddressBuilderTests
{
    [Fact]
    public void Build_WithMinimalData_ReturnsAddressWithDefaults()
    {
        // Arrange & Act
        var address = new AddressBuilder().Build();

        // Assert
        Assert.NotNull(address);
        Assert.Equal(string.Empty, address.Street);
        Assert.Equal(string.Empty, address.City);
        Assert.Equal(string.Empty, address.State);
        Assert.Equal(string.Empty, address.Zip);
    }

    [Fact]
    public void Street_SetsAddressStreet_ReturnsBuilder()
    {
        // Arrange
        var builder = new AddressBuilder();

        // Act
        var result = builder.Street("123 Main St");
        var address = result.Build();

        // Assert
        Assert.Same(builder, result);
        Assert.Equal("123 Main St", address.Street);
    }

    [Fact]
    public void City_SetsAddressCity_ReturnsBuilder()
    {
        // Arrange
        var builder = new AddressBuilder();

        // Act
        var result = builder.City("Springfield");
        var address = result.Build();

        // Assert
        Assert.Same(builder, result);
        Assert.Equal("Springfield", address.City);
    }

    [Fact]
    public void State_SetsAddressState_ReturnsBuilder()
    {
        // Arrange
        var builder = new AddressBuilder();

        // Act
        var result = builder.State("IL");
        var address = result.Build();

        // Assert
        Assert.Same(builder, result);
        Assert.Equal("IL", address.State);
    }

    [Fact]
    public void Zip_SetsAddressZip_ReturnsBuilder()
    {
        // Arrange
        var builder = new AddressBuilder();

        // Act
        var result = builder.Zip("62701");
        var address = result.Build();

        // Assert
        Assert.Same(builder, result);
        Assert.Equal("62701", address.Zip);
    }

    [Fact]
    public void FluentChaining_BuildsCompleteAddress()
    {
        // Act
        var address = new AddressBuilder()
            .Street("789 Pine St")
            .City("Chicago")
            .State("IL")
            .Zip("60601")
            .Build();

        // Assert
        Assert.Equal("789 Pine St", address.Street);
        Assert.Equal("Chicago", address.City);
        Assert.Equal("IL", address.State);
        Assert.Equal("60601", address.Zip);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123 Main St")]
    [InlineData("456 Oak Avenue, Apt 2B")]
    [InlineData("789 Very Long Street Name That Might Be Used In Real Life")]
    public void Street_WithVariousValues_SetsCorrectly(string street)
    {
        // Act
        var address = new AddressBuilder().Street(street).Build();

        // Assert
        Assert.Equal(street, address.Street);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Chicago")]
    [InlineData("New York")]
    [InlineData("San Francisco")]
    [InlineData("Very Long City Name That Exists Somewhere")]
    public void City_WithVariousValues_SetsCorrectly(string city)
    {
        // Act
        var address = new AddressBuilder().City(city).Build();

        // Assert
        Assert.Equal(city, address.City);
    }

    [Theory]
    [InlineData("")]
    [InlineData("IL")]
    [InlineData("CA")]
    [InlineData("NY")]
    [InlineData("Texas")]
    public void State_WithVariousValues_SetsCorrectly(string state)
    {
        // Act
        var address = new AddressBuilder().State(state).Build();

        // Assert
        Assert.Equal(state, address.State);
    }

    [Theory]
    [InlineData("")]
    [InlineData("12345")]
    [InlineData("12345-6789")]
    [InlineData("60601")]
    [InlineData("90210")]
    public void Zip_WithVariousValues_SetsCorrectly(string zip)
    {
        // Act
        var address = new AddressBuilder().Zip(zip).Build();

        // Assert
        Assert.Equal(zip, address.Zip);
    }

    [Fact]
    public void Build_CalledMultipleTimes_ReturnsSameAddressInstance()
    {
        // Arrange
        var builder = new AddressBuilder()
            .Street("123 Test St")
            .City("Test City");

        // Act
        var address1 = builder.Build();
        var address2 = builder.Build();

        // Assert
        Assert.Same(address1, address2);
        Assert.Equal("123 Test St", address1.Street);
        Assert.Equal("Test City", address1.City);
    }
}