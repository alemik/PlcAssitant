using PlcAssistant.Server;
using PlcAssistant.Domain.Interfaces;
using PlcAssistant.Infrastructure.Repositories;
using PlcAssistant.Infrastructure.Services;

var builder = Host.CreateApplicationBuilder(args);

// Configure database path
var dbPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "PlcAssistant",
    "plc.db");

// Ensure directory exists
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

var connectionString = $"Filename={dbPath};Connection=shared";

// Register repositories
builder.Services.AddSingleton<IPlcMemoryAddressRepository>(
    sp => new PlcMemoryAddressRepository(connectionString));
builder.Services.AddSingleton<IServerConfigurationRepository>(
    sp => new ServerConfigurationRepository(connectionString));

// Register PLC server service
builder.Services.AddSingleton<IPlcServerService, PlcServerService>();

// Register data seeder
builder.Services.AddSingleton<DataSeeder>();

// Register hosted service
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

// Optionally seed initial data
var seedData = args.Contains("--seed");
if (seedData)
{
    var seeder = host.Services.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
    Console.WriteLine("Database seeded with sample data.");
}

host.Run();
