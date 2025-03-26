using FluentAssertions;
using PettingZooApi.Web.Data;

namespace PettingZooApi.Tests.IntegrationTests;

public class AnimalControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AnimalControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    void ReinitializeDb()
    {
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<PettingZooApiContext>();
        Utilities.ReinitializeDbForTests(db);
    }

    [Fact]
    public async void ListAnimals()
    {
        // Arrange
        ReinitializeDb();

        // Act
        var response = await _client.GetAsync("/api/animals");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        // Using wildcards so we don't have to match JSON format exactly
        content.Should().Match("*animalId*:*1*");
        content.Should().Match("*name*:*test*");
        content.Should().NotContain("species");
    }

    [Fact]
    public async void FeedAnimal()
    {
        // Arrange
        ReinitializeDb();

        // Act
        var response = await _client.GetAsync("/api/animals/1/feed?foodName=carrots");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        content.Should().Contain("thanks you for the food");
    }
}