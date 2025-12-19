using LiteDB;
using PlcAssistant.Domain.Entities;
using PlcAssistant.Domain.Interfaces;

namespace PlcAssistant.Infrastructure.Repositories;

/// <summary>
/// LiteDB implementation of server configuration repository
/// </summary>
public class ServerConfigurationRepository : IServerConfigurationRepository
{
    private const string CollectionName = "serverConfiguration";
    private readonly string _connectionString;

    public ServerConfigurationRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<ServerConfiguration> GetConfigurationAsync()
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(_connectionString);
            var collection = db.GetCollection<ServerConfiguration>(CollectionName);
            var config = collection.FindAll().FirstOrDefault();
            
            if (config == null)
            {
                config = new ServerConfiguration();
                collection.Insert(config);
            }
            
            return config;
        });
    }

    public async Task UpdateConfigurationAsync(ServerConfiguration configuration)
    {
        await Task.Run(() =>
        {
            using var db = new LiteDatabase(_connectionString);
            var collection = db.GetCollection<ServerConfiguration>(CollectionName);
            configuration.LastModifiedAt = DateTime.UtcNow;
            collection.Update(configuration);
        });
    }
}
