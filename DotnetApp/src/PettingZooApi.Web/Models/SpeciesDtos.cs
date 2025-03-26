namespace PettingZooApi.Web.Models;

public class CreateSpeciesDto
{
    public required string Name { get; set; } = null!;
    public required string Diet { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdateSpeciesDto
{
    public string? Diet { get; set; }
    public string? Description { get; set; }
}