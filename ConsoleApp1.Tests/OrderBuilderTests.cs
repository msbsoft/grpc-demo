using ConsoleApp1;
using Xunit;

namespace ConsoleApp1.Tests;

public class OrderBuilderTests
{
    [Fact]
    public void Create_ReturnsNewOrderBuilder()
    {
        // Act
        var builder = OrderBuilder.Create();

        // Assert
        Assert.NotNull(builder);
    }

    [Fact]
    public void Build_WithMinimalData_ReturnsOrderWithDefaults()
    {
        // Arrange & Act
        var order = OrderBuilder.Create().Build();

        // Assert
        Assert.NotNull(order);
        Assert.Equal(0, order.Id);
        Assert.Equal(0, order.Price);
        Assert.Null(order.ShippedTo);
    }

    [Fact]
    public void Id_SetsOrderId_ReturnsBuilder()
    {
        // Arrange
        var builder = OrderBuilder.Create();

        // Act
        var result = builder.Id(123);
        var order = result.Build();

        // Assert
        Assert.Same(builder, result);
        Assert.Equal(123, order.Id);
    }

    [Fact]
    public void Price_SetsOrderPrice_ReturnsBuilder()
    {
        // Arrange
        var builder = OrderBuilder.Create();

        // Act
        var result = builder.Price(99.99m);
        var order = result.Build();

        // Assert
        Assert.Same(builder, result);
        Assert.Equal(99.99m, order.Price);
    }

    [Fact]
    public void ShippedTo_WithAddressBuilder_SetsAddress_ReturnsBuilder()
    {
        // Arrange
        var builder = OrderBuilder.Create();

        // Act
        var result = builder.ShippedTo(b =>
            b.Street("123 Main St")
             .City("Anytown")
             .State("CA")
             .Zip("12345"));
        var order = result.Build();

        // Assert
        Assert.Same(builder, result);
        Assert.NotNull(order.ShippedTo);
        Assert.Equal("123 Main St", order.ShippedTo.Street);
        Assert.Equal("Anytown", order.ShippedTo.City);
        Assert.Equal("CA", order.ShippedTo.State);
        Assert.Equal("12345", order.ShippedTo.Zip);
    }

    [Fact]
    public void FluentChaining_BuildsCompleteOrder()
    {
        // Act
        var order = OrderBuilder.Create()
            .Id(456)
            .Price(149.99m)
            .ShippedTo(b =>
                b.Street("456 Oak Ave")
                 .City("Springfield")
                 .State("IL")
                 .Zip("62701"))
            .Build();

        // Assert
        Assert.Equal(456, order.Id);
        Assert.Equal(149.99m, order.Price);
        Assert.NotNull(order.ShippedTo);
        Assert.Equal("456 Oak Ave", order.ShippedTo.Street);
        Assert.Equal("Springfield", order.ShippedTo.City);
        Assert.Equal("IL", order.ShippedTo.State);
        Assert.Equal("62701", order.ShippedTo.Zip);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    [InlineData(-1)]
    public void Id_WithVariousValues_SetsCorrectly(int id)
    {
        // Act
        var order = OrderBuilder.Create().Id(id).Build();

        // Assert
        Assert.Equal(id, order.Id);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0.01)]
    [InlineData(999.99)]
    [InlineData(1000000.50)]
    public void Price_WithVariousValues_SetsCorrectly(decimal price)
    {
        // Act
        var order = OrderBuilder.Create().Price(price).Build();

        // Assert
        Assert.Equal(price, order.Price);
    }
}