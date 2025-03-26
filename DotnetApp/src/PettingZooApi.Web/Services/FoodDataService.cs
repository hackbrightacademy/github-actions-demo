namespace PettingZooApi.Web.Services;

public interface IFoodDataService
{
    Task<string> GetFoodCategory(string foodName);
}

public class FoodDataService : IFoodDataService
{
    private readonly HttpClient _client;
    public FoodDataService(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("https://api.nal.usda.gov/fdc/v1/");
    }

    public async Task<string> GetFoodCategory(string foodName)
    {
        var response = await _client.GetAsync($"foods/search?query={foodName}&pageSize=1&dataType=Foundation");
        response.EnsureSuccessStatusCode();
        var foodData = await response.Content
            .ReadFromJsonAsync<FoodDataServiceListFoodsResponse>();
        return foodData?.Foods.FirstOrDefault()?.FoodCategory ?? "Unknown";
    }
}

public class FoodDataServiceListFoodsResponse
{
    public List<Food> Foods { get; set; } = [];

    public class Food
    {
        public int FdcId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string FoodCategory { get; set; } = string.Empty;
    }
}
