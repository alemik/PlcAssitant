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

// Register hosted service
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
