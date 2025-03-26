using System.ComponentModel.DataAnnotations;

namespace PettingZooApi.Web.Models;

public class ListAnimalDto
{
    public int AnimalId { get; set; }
    public string Name { get; set; } = null!;
}

public class CreateAnimalDto
{
    [MinLength(1)] // Ensure the new animal's name isn't empty
    public required string Name { get; set; } = null!;
    public required string SpeciesName { get; set; } = null!;
}

public class UpdateAnimalDto
{
    public string? Name { get; set; }
}