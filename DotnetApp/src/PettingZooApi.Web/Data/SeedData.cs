using Microsoft.EntityFrameworkCore;
using PettingZooApi.Web.Models;

namespace PettingZooApi.Web.Data;

public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var db = new PettingZooApiContext(
            serviceProvider.GetRequiredService<DbContextOptions<PettingZooApiContext>>()))
        {
            db.Database.EnsureCreated();
        }
        SeedSpecies(serviceProvider);
        SeedAnimals(serviceProvider);
    }

    static void SeedSpecies(IServiceProvider serviceProvider)
    {
        using (var db = new PettingZooApiContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<PettingZooApiContext>>()))
        {
            if (db.Species.Any())
            {
                return;
            }

            db.AddRange(
                new Species
                {
                    Name = "rabbit",
                    Diet = "herbivore",
                    Description = "Rabbits are small mammals in the family Leporidae (which also includes the hares), which is in the order Lagomorpha (which also includes pikas)."
                },
                new Species
                {
                    Name = "capybara",
                    Diet = "herbivore",
                    Description = "The capybara is the largest living rodent, native to South America."
                },
                new Species
                {
                    Name = "cat",
                    Diet = "carnivore",
                    Description = "The domestic cat is a small, typically furry, carnivorous mammal."
                }
            );

            db.SaveChanges();
        }
    }

    static void SeedAnimals(IServiceProvider serviceProvider)
    {
        using (var db = new PettingZooApiContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<PettingZooApiContext>>()))
        {
            if (db.Animals.Any())
            {
                return;
            }

            var rabbit = db.Species.Single(sp => sp.Name == "rabbit");
            db.AddRange(
                new Animal { Name = "Roger", Species = rabbit },
                new Animal { Name = "Ray", Species = rabbit },
                new Animal { Name = "Renna", Species = rabbit }
            );

            var capy = db.Species.Single(sp => sp.Name == "capybara");
            db.Add(new Animal { Name = "Clyde", Species = capy });

            var cat = db.Species.Single(sp => sp.Name == "cat");
            db.Add(new Animal { Name = "Cleo", Species = cat });

            db.SaveChanges();
        }
    }
}
