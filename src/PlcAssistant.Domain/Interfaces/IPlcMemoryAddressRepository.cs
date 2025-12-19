using PlcAssistant.Domain.Entities;

namespace PlcAssistant.Domain.Interfaces;

/// <summary>
/// Repository interface for PLC memory addresses
/// </summary>
public interface IPlcMemoryAddressRepository
{
    Task<IEnumerable<PlcMemoryAddress>> GetAllAsync();
    Task<PlcMemoryAddress?> GetByIdAsync(Guid id);
    Task<PlcMemoryAddress> AddAsync(PlcMemoryAddress address);
    Task UpdateAsync(PlcMemoryAddress address);
    Task DeleteAsync(Guid id);
    Task<PlcMemoryAddress?> GetByAddressAsync(string address);
}
