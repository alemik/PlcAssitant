using Microsoft.Extensions.Logging;
using PlcAssistant.Domain.Interfaces;
using PlcAssistant.Infrastructure.Repositories;
using PlcAssistant.Infrastructure.Services;

namespace PlcAssistant.MauiApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Add Blazor WebView
        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // Configure database path
        var dbPath = Path.Combine(
            FileSystem.AppDataDirectory,
            "plc.db");

        var connectionString = $"Filename={dbPath};Connection=shared";

        // Register repositories
        builder.Services.AddSingleton<IPlcMemoryAddressRepository>(
            sp => new PlcMemoryAddressRepository(connectionString));
        builder.Services.AddSingleton<IServerConfigurationRepository>(
            sp => new ServerConfigurationRepository(connectionString));

        // Register PLC server service
        builder.Services.AddSingleton<IPlcServerService, PlcServerService>();

        // Add Syncfusion Blazor services
        builder.Services.AddSyncfusionBlazor();

        return builder.Build();
    }
}
