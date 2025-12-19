using LiteDB;

namespace PlcAssistant.Infrastructure.Data;

/// <summary>
/// LiteDB database context for PLC Assistant
/// </summary>
public class PlcDbContext : IDisposable
{
    private readonly LiteDatabase _database;

    public PlcDbContext(string connectionString)
    {
        _database = new LiteDatabase(connectionString);
    }

    public ILiteCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }

    public void Dispose()
    {
        _database?.Dispose();
    }
}
