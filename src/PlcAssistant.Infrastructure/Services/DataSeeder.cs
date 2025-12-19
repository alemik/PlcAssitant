using PlcAssistant.Domain.Entities;
using PlcAssistant.Domain.Interfaces;

namespace PlcAssistant.Infrastructure.Services;

/// <summary>
/// Service to seed initial data for testing and demonstration
/// </summary>
public class DataSeeder
{
    private readonly IPlcMemoryAddressRepository _addressRepository;
    private readonly IServerConfigurationRepository _configRepository;

    public DataSeeder(
        IPlcMemoryAddressRepository addressRepository,
        IServerConfigurationRepository configRepository)
    {
        _addressRepository = addressRepository;
        _configRepository = configRepository;
    }

    public async Task SeedAsync()
    {
        // Check if we already have data
        var existingAddresses = await _addressRepository.GetAllAsync();
        if (existingAddresses.Any())
        {
            return; // Data already seeded
        }

        // Seed sample memory addresses
        var addresses = new[]
        {
            new PlcMemoryAddress
            {
                Name = "Temperature Sensor 1",
                Address = "DB1.DBD0",
                DataType = PlcDataType.Real,
                CurrentValue = "20.5",
                Description = "Temperature sensor in zone 1",
                IsReadOnly = false
            },
            new PlcMemoryAddress
            {
                Name = "Motor Status",
                Address = "DB1.DBX4.0",
                DataType = PlcDataType.Bool,
                CurrentValue = "false",
                Description = "Motor ON/OFF status",
                IsReadOnly = false
            },
            new PlcMemoryAddress
            {
                Name = "Production Counter",
                Address = "DB1.DBD8",
                DataType = PlcDataType.DInt,
                CurrentValue = "0",
                Description = "Total production count",
                IsReadOnly = false
            },
            new PlcMemoryAddress
            {
                Name = "Pressure Sensor",
                Address = "DB2.DBD0",
                DataType = PlcDataType.Real,
                CurrentValue = "1.013",
                Description = "Pressure in bar",
                IsReadOnly = false
            },
            new PlcMemoryAddress
            {
                Name = "Emergency Stop",
                Address = "I0.0",
                DataType = PlcDataType.Bool,
                CurrentValue = "false",
                Description = "Emergency stop button",
                IsReadOnly = true
            }
        };

        foreach (var address in addresses)
        {
            await _addressRepository.AddAsync(address);
        }

        // Ensure configuration exists with default values
        var config = await _configRepository.GetConfigurationAsync();
        if (config.Port == 0)
        {
            config.Port = 502;
            config.IpAddress = "127.0.0.1";
            config.UpdateIntervalMs = 100;
            config.AutoStart = false;
            await _configRepository.UpdateConfigurationAsync(config);
        }
    }
}
