using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PettingZooApi.Web.Data;
using PettingZooApi.Web.Models;

namespace PettingZooApi.Web.Controllers;

[Route("api/species")]
[ApiController]
[Produces("application/json")]
public class SpeciesController : Controller
{

    private readonly PettingZooApiContext _db;
    public SpeciesController(PettingZooApiContext dbContext)
    {
        _db = dbContext;
    }

    /// <summary>
    /// List all species
    /// </summary>
    /// <returns>A list of species</returns>
    /// <response code="200">Returns a list of species</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Species>>> ListSpecies()
    {
        var species = await _db.Species.ToListAsync();
        return Ok(species);
    }

    /// <summary>
    /// Get a species by ID
    /// </summary>
    /// <param name="id">The species ID</param>
    /// <returns>A species with the given ID</returns>
    /// <response code="200">Returns a species with the given ID</response>
    /// <response code="404">If a species with the given ID is not found</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<Species>> GetSpeciesById(int id)
    {
        var species = await _db.Species.SingleOrDefaultAsync(species => species.SpeciesId == id);
        if (species == null)
        {
            return NotFound($"Species with ID {id} is not found");
        }
        return Ok(species);
    }

    /// <summary>
    /// Get the animals of a species by species ID
    /// </summary>
    /// <param name="id">The species ID</param>
    /// <returns>A list of animals</returns>
    /// <response code="200">Returns a list of animals</response>
    [HttpGet("{id}/animals")]
    public async Task<ActionResult<IEnumerable<Animal>>> GetAnimalsBySpeciesId(int id)
    {
        var animals = await _db.Animals
            .Where(animal => animal.SpeciesId == id)
            .Select(animal => new ListAnimalDto
            {
                AnimalId = animal.AnimalId,
                Name = animal.Name
            })
            .ToListAsync();
        return Ok(animals);
    }

    /// <summary>
    /// Create a species
    /// </summary>
    /// <param name="createSpecies">Fields used to create the species</param>
    /// <returns>A newly created species</returns>
    /// <response code="201">Returns the newly created species</response>
    [HttpPost]
    public async Task<ActionResult<Species>> CreateSpecies(CreateSpeciesDto createSpecies)
    {
        var species = new Species
        {
            Name = createSpecies.Name,
            Diet = createSpecies.Diet
        };

        if (createSpecies.Description != null)
        {
            species.Description = createSpecies.Description;
        }

        _db.Species.Add(species);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSpeciesById), new { id = species.SpeciesId }, species);
    }

    /// <summary>
    /// Delete a species by ID
    /// </summary>
    /// <param name="id">The species ID to delete</param>
    /// <returns></returns>
    /// <response code="204">No content</response>
    /// <response code="404">If a species with the given ID is not found</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSpecies(int id)
    {
        var species = await _db.Species.FindAsync(id);

        if (species == null)
        {
            return NotFound($"Species with ID {id} is not found");
        }

        _db.Species.Remove(species);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Update a species by ID
    /// </summary>
    /// <param name="id">The species ID to update</param>
    /// <param name="updateSpecies">Fields used to update the species</param>
    /// <returns></returns>
    /// <response code="200">Returns the updated species</response>
    /// <response code="404">If a species with the given ID is not found</response>
    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateSpecies(int id, UpdateSpeciesDto updateSpecies)
    {
        var species = await _db.Species.FindAsync(id);

        if (species == null)
        {
            return NotFound($"Species with ID {id} is not found");
        }
        // Only update the fields that are not null
        species.Diet = updateSpecies.Diet ?? species.Diet;
        species.Description = updateSpecies.Description ?? species.Description;
        await _db.SaveChangesAsync();

        return Ok(species);
    }
}