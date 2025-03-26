using PettingZooApi.Web.Data;
using PettingZooApi.Web.Models;

namespace PettingZooApi.Tests;

public static class Utilities
{
    public static void InitializeDbForTests(PettingZooApiContext db)
    {
        // Primary key values are hardcoded for predictable results
        var testSpecies = new Species { SpeciesId = 1, Name = "test", Diet = "herbivore" };
        db.Species.Add(testSpecies);
        db.SaveChanges();

        var testAnimal = new Animal { AnimalId = 1, Name = "test", SpeciesId = 1 };
        db.Animals.Add(testAnimal);
        db.SaveChanges();
    }

    public static void ReinitializeDbForTests(PettingZooApiContext db)
    {
        db.Species.RemoveRange(db.Species);
        db.Animals.RemoveRange(db.Animals);
        InitializeDbForTests(db);
    }

}