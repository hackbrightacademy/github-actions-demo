using Microsoft.EntityFrameworkCore;
using PettingZooApi.Web.Controllers;
using PettingZooApi.Web.Models;
using PettingZooApi.Web.Data;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

public class SpeciesControllerTests
{
    PettingZooApiContext CreateContext()
    {
        var contextOptions = new DbContextOptionsBuilder<PettingZooApiContext>()
            .UseNpgsql("Host=localhost;Database=pettingzoo")
            .Options;
        return new PettingZooApiContext(contextOptions);
    }

    [Fact(Skip = "Don't actually test the production database")]
    public async void CreateSpecies_ReturnsValue()
    {
        using var dbContext = CreateContext();
        var controller = new SpeciesController(dbContext);
        var dto = new CreateSpeciesDto { Name = "test", Diet = "herbivore" };

        ActionResult<Species> result = await controller.CreateSpecies(dto);

        result.Should().NotBeNull();
        // Retrieving the value from the ActionResult
        var species = (result.Result as CreatedAtActionResult)!.Value as Species;
        species!.Name.Should().Be("test");
    }
}