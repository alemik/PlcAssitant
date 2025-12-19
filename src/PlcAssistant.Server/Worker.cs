using PlcAssistant.Domain.Interfaces;

namespace PlcAssistant.Server;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IPlcServerService _plcServerService;
    private readonly IServerConfigurationRepository _configRepository;

    public Worker(
        ILogger<Worker> logger,
        IPlcServerService plcServerService,
        IServerConfigurationRepository configRepository)
    {
        _logger = logger;
        _plcServerService = plcServerService;
        _configRepository = configRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PLC Assistant Server starting...");

        try
        {
            var config = await _configRepository.GetConfigurationAsync();
            
            if (config.AutoStart)
            {
                _logger.LogInformation("Auto-starting PLC server...");
                await _plcServerService.StartAsync();
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                var status = await _plcServerService.GetStatusAsync();
                _logger.LogInformation("PLC Server Status: {status}", status);
                await Task.Delay(5000, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PLC Assistant Server");
        }
        finally
        {
            if (_plcServerService.IsRunning)
            {
                _logger.LogInformation("Stopping PLC server...");
                await _plcServerService.StopAsync();
            }
        }

        _logger.LogInformation("PLC Assistant Server stopped");
    }
}
