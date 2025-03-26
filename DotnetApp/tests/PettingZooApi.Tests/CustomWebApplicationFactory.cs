using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PettingZooApi.Web.Services;
using PettingZooApi.Web.Tests;
using PettingZooApi.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace PettingZooApi.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            builder.UseEnvironment("Development");

            // Replace the database connection with an in-memory SQLite db
            services.RemoveAll<DbConnection>();
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                return connection;
            });

            // Replace the database context with one that uses the in-memory SQLite db
            services.RemoveAll<DbContextOptions<PettingZooApiContext>>();
            services.AddDbContext<PettingZooApiContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });

            // Replace the food data service with a stub
            services.RemoveAll<IFoodDataService>();
            services.AddScoped<IFoodDataService, StubFoodDataService>();
        });
    }
}