using PlcAssistant.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace PlcAssistant.Infrastructure.Services;

/// <summary>
/// PLC server service implementation
/// </summary>
public class PlcServerService : IPlcServerService
{
    private readonly ILogger<PlcServerService> _logger;
    private readonly IPlcMemoryAddressRepository _memoryRepository;
    private bool _isRunning;
    private CancellationTokenSource? _cancellationTokenSource;
    private Task? _serverTask;

    public bool IsRunning => _isRunning;

    public PlcServerService(
        ILogger<PlcServerService> logger,
        IPlcMemoryAddressRepository memoryRepository)
    {
        _logger = logger;
        _memoryRepository = memoryRepository;
    }

    public async Task StartAsync()
    {
        if (_isRunning)
        {
            _logger.LogWarning("PLC Server is already running");
            return;
        }

        _logger.LogInformation("Starting PLC Server...");
        _cancellationTokenSource = new CancellationTokenSource();
        _isRunning = true;

        _serverTask = Task.Run(async () => await RunServerLoopAsync(_cancellationTokenSource.Token));
        
        _logger.LogInformation("PLC Server started successfully");
        await Task.CompletedTask;
    }

    public async Task StopAsync()
    {
        if (!_isRunning)
        {
            _logger.LogWarning("PLC Server is not running");
            return;
        }

        _logger.LogInformation("Stopping PLC Server...");
        _cancellationTokenSource?.Cancel();
        
        if (_serverTask != null)
        {
            await _serverTask;
        }

        _isRunning = false;
        _logger.LogInformation("PLC Server stopped successfully");
    }

    public async Task<string> GetStatusAsync()
    {
        var addresses = await _memoryRepository.GetAllAsync();
        var addressCount = addresses.Count();
        
        return _isRunning 
            ? $"Running - {addressCount} memory addresses configured" 
            : $"Stopped - {addressCount} memory addresses configured";
    }

    private async Task RunServerLoopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("PLC Server loop started");

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Simulate PLC operations
                await Task.Delay(100, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("PLC Server loop cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PLC Server loop");
        }
    }
}
