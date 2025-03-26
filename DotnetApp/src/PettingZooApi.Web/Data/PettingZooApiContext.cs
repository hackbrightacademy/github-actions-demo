using Microsoft.EntityFrameworkCore;
using PettingZooApi.Web.Models;

namespace PettingZooApi.Web.Data;

public class PettingZooApiContext : DbContext
{
    public PettingZooApiContext(DbContextOptions<PettingZooApiContext> options)
        : base(options)
    {
    }

    public DbSet<Animal> Animals { get; set; }
    public DbSet<Species> Species { get; set; }
}
