using System.ComponentModel.DataAnnotations.Schema;

namespace PettingZooApi.Web.Models;

[Table("species")]
public class Species
{
    [Column("species_id")]
    public int SpeciesId { get; set; }

    [Column("name", TypeName = "varchar(120)")]
    public required string Name { get; set; }

    [Column("diet", TypeName = "varchar(10)")]
    public required string Diet { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; } = string.Empty;
}
