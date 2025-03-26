using PettingZooApi.Web.Services;

namespace PettingZooApi.Web.Tests;

public class StubFoodDataService : IFoodDataService
{
    public Task<string> GetFoodCategory(string foodName)
    {
        return Task.FromResult("Vegetable");
    }
}