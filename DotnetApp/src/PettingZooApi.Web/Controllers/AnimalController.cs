using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PettingZooApi.Web.Data;
using PettingZooApi.Web.Models;
using PettingZooApi.Web.Services;

namespace PettingZooApi.Web.Controllers;

[Route("api/animals")]
[ApiController]
public class AnimalController : Controller
{
    private readonly PettingZooApiContext _db;

    public AnimalController(PettingZooApiContext dbContext)
    {
        _db = dbContext;
    }

    /// GET: /api/animals
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListAnimalDto>>> ListAnimals()
    {
        var animals = await _db.Animals
            .Select(animal => new ListAnimalDto
            {
                AnimalId = animal.AnimalId,
                Name = animal.Name
            })
            .ToListAsync();
        return Ok(animals);
    }

    /// GET: /api/animals/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Animal>> GetAnimalById(int id)
    {
        var animal = await _db.Animals
            .Include(animal => animal.Species)
            .SingleOrDefaultAsync(animal => animal.AnimalId == id);

        if (animal == null)
        {
            return NotFound($"Animal with ID {id} is not found");
        }

        return Ok(animal);
    }

    /// POST: /api/animals
    [HttpPost]
    public async Task<ActionResult<Animal>> CreateAnimal(
        [FromBody] CreateAnimalDto createAnimal)
    {
        var species = await _db.Species
            .SingleOrDefaultAsync(species => species.Name == createAnimal.SpeciesName);

        if (species == null)
        {
            return BadRequest($"Species '{createAnimal.SpeciesName}' is not found");
        }

        var animal = new Animal
        {
            Name = createAnimal.Name,
            Species = species
        };
        _db.Animals.Add(animal);
        await _db.SaveChangesAsync();

        // Reuse the GetAnimalById method to return the newly created animal
        // and set HTTP status code 201 (Created)
        return CreatedAtAction(nameof(GetAnimalById), new { id = animal.AnimalId }, animal);
    }

    /// DELETE: /api/animals/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAnimal(int id)
    {
        var animal = await _db.Animals.FindAsync(id);
        if (animal == null)
        {
            return NotFound($"Animal with ID {id} is not found");
        }

        _db.Animals.Remove(animal);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    /// GET: /api/animals/search?name={name}
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Animal>>> SearchAnimals(
        [FromQuery] string name)
    {
        var animals = await _db.Animals
            .Where(animal => animal.Name.Contains(name))
            .Select(animal => new ListAnimalDto
            {
                AnimalId = animal.AnimalId,
                Name = animal.Name
            })
            .ToListAsync();
        return Ok(animals);
    }

    /// PATCH: /api/animals/{id}
    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateAnimal(
        int id, [FromBody] UpdateAnimalDto updateAnimal)
    {
        var animal = await _db.Animals.FindAsync(id);
        if (animal == null)
        {
            return NotFound($"Animal with ID {id} is not found");
        }

        // Only update the fields that are not null
        animal.Name = updateAnimal.Name ?? animal.Name;

        await _db.SaveChangesAsync();

        return Ok(animal);
    }

    [HttpGet("{id}/feed")]
    public async Task<ActionResult<string>> FeedAnimal(
        int id,
        [FromQuery] string foodName,
        [FromServices] IFoodDataService _foodDataService)
    {
        var animal = await _db.Animals.FindAsync(id);
        if (animal == null)
        {
            return NotFound($"Animal with ID {id} is not found");
        }
        await _db.Entry(animal).Reference(animal => animal.Species).LoadAsync();

        var foodCategory = await _foodDataService.GetFoodCategory(foodName);
        if (animal.Species.Diet == "herbivore" && !foodCategory.Contains("Vegetable"))
        {
            return BadRequest($"Animal with ID {id} is an herbivore and only eats vegetables");
        }

        return Ok($"{animal.Name} thanks you for the food!");
    }
}
