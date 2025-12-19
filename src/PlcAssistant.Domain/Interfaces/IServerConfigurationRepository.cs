using PlcAssistant.Domain.Entities;

namespace PlcAssistant.Domain.Interfaces;

/// <summary>
/// Repository interface for server configuration
/// </summary>
public interface IServerConfigurationRepository
{
    Task<ServerConfiguration> GetConfigurationAsync();
    Task UpdateConfigurationAsync(ServerConfiguration configuration);
}
