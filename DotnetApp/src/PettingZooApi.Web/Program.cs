using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

using PettingZooApi.Web.Data;
using PettingZooApi.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddTransient<ApiKeyMessageHandler>();
builder.Services.AddHttpClient<IFoodDataService, FoodDataService>()
    .AddHttpMessageHandler<ApiKeyMessageHandler>();

builder.Services.Configure<ApiKeyMessageHandlerOptions>(
    builder.Configuration.GetSection("FoodDataService"));

builder.Services.AddSwaggerGen(opts =>
{
  opts.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "Petting Zoo API",
    Version = "v1",
    Description = "A simple API for a petting zoo"
  });
  opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "PettingZooApi.Web.xml"));
});

builder.Services.AddDbContext<PettingZooApiContext>(options =>
{
  // Read connection string from appsettings.json
  options.UseNpgsql(builder.Configuration.GetConnectionString("PettingZooApiContext"));
});
var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// Explicitly define routes
app.MapControllers();

app.Run();

// .NET automatically generates a Program class that's internal by default.
// The line below makes the class public so it can be used by our tests.
public partial class Program { }

// Please merge this ingenious comment