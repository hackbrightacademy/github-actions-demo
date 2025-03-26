using FluentAssertions;
using PettingZooApi.Util;

namespace PettingZooApi.Tests.UnitTests;

public class ConverterTests
{
    [Fact]
    public void FeedToCalories_ConvertsCorrectly()
    {
        // Arrange
        var feedAmount = 100m;
        var caloriesPer100g = 50m;
        var decimalPlaces = 2;
        var expected = 50m;

        // Act
        var result = Converters.FeedToCalories(
            feedAmount, caloriesPer100g, decimalPlaces);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(100, 50, 50)]
    [InlineData(200, 50, 100)]
    [InlineData(0, 100, 0)]
    public void FeedToCalories_ConvertsCorrectly2(
        decimal feedAmount,
        decimal caloriesPer100g,
        decimal expected)
    {
        var decimalPlaces = 2;

        var result = Converters.FeedToCalories(feedAmount, caloriesPer100g, decimalPlaces);

        // Using FluentAssertions instead of xUnit's Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void FeedToCalories_ThrowsException_WhenCaloriesPer100gIsZero()
    {
        var feedAmount = 100m;
        var caloriesPer100g = 0m;
        var decimalPlaces = 2;

        Action act = () => Converters.FeedToCalories(
            feedAmount, caloriesPer100g, decimalPlaces);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Calories per 100g must be greater than 0*");

        // Using xUnit's Assert:
        // var ex = Assert.Throws<ArgumentException>(
        //     () => Converters.FeedToCalories(feedAmount, caloriesPer100g, decimalPlaces));
        // Assert.Contains("Calories per 100g must be greater than 0", ex.Message);
    }
}