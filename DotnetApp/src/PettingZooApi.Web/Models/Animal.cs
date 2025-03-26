using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PettingZooApi.Web.Models;

[Table("animals")]
public class Animal
{
    [Column("animal_id")]
    public int AnimalId { get; set; }

    [Column("name", TypeName = "varchar(120)")]
    public required string Name { get; set; }

    [Column("species_id")]
    [ForeignKey("Species")]
    [Required]
    [JsonIgnore]
    public int SpeciesId { get; set; }
    public Species Species { get; set; } = null!;
}
