namespace PlcAssistant.Domain.Interfaces;

/// <summary>
/// Interface for PLC server control
/// </summary>
public interface IPlcServerService
{
    bool IsRunning { get; }
    Task StartAsync();
    Task StopAsync();
    Task<string> GetStatusAsync();
}
