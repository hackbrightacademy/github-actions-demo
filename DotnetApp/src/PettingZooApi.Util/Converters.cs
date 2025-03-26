namespace PettingZooApi.Util;

public class Converters
{
    public static decimal FeedToCalories(
        decimal feedAmount,
        decimal caloriesPer100g,
        int decimalPlaces)
    {
        if (caloriesPer100g <= 0)
        {
            throw new ArgumentException(
                "Calories per 100g must be greater than 0",
                nameof(caloriesPer100g));
        }

        var cals = feedAmount * caloriesPer100g / 100;
        return decimal.Round(cals, decimalPlaces);
    }
}
