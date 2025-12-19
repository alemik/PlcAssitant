using LiteDB;
using PlcAssistant.Domain.Entities;
using PlcAssistant.Domain.Interfaces;

namespace PlcAssistant.Infrastructure.Repositories;

/// <summary>
/// LiteDB implementation of PLC memory address repository
/// </summary>
public class PlcMemoryAddressRepository : IPlcMemoryAddressRepository
{
    private const string CollectionName = "memoryAddresses";
    private readonly string _connectionString;

    public PlcMemoryAddressRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<PlcMemoryAddress>> GetAllAsync()
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(_connectionString);
            var collection = db.GetCollection<PlcMemoryAddress>(CollectionName);
            return collection.FindAll().ToList();
        });
    }

    public async Task<PlcMemoryAddress?> GetByIdAsync(Guid id)
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(_connectionString);
            var collection = db.GetCollection<PlcMemoryAddress>(CollectionName);
            return collection.FindById(id);
        });
    }

    public async Task<PlcMemoryAddress> AddAsync(PlcMemoryAddress address)
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(_connectionString);
            var collection = db.GetCollection<PlcMemoryAddress>(CollectionName);
            collection.Insert(address);
            return address;
        });
    }

    public async Task UpdateAsync(PlcMemoryAddress address)
    {
        await Task.Run(() =>
        {
            using var db = new LiteDatabase(_connectionString);
            var collection = db.GetCollection<PlcMemoryAddress>(CollectionName);
            address.LastModifiedAt = DateTime.UtcNow;
            collection.Update(address);
        });
    }

    public async Task DeleteAsync(Guid id)
    {
        await Task.Run(() =>
        {
            using var db = new LiteDatabase(_connectionString);
            var collection = db.GetCollection<PlcMemoryAddress>(CollectionName);
            collection.Delete(id);
        });
    }

    public async Task<PlcMemoryAddress?> GetByAddressAsync(string address)
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(_connectionString);
            var collection = db.GetCollection<PlcMemoryAddress>(CollectionName);
            return collection.FindOne(x => x.Address == address);
        });
    }
}
