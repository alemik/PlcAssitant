namespace PlcAssistant.Domain.Entities;

/// <summary>
/// Server configuration settings
/// </summary>
public class ServerConfiguration
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Port { get; set; } = 502;
    public string IpAddress { get; set; } = "127.0.0.1";
    public int UpdateIntervalMs { get; set; } = 100;
    public bool AutoStart { get; set; }
    public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;
}
