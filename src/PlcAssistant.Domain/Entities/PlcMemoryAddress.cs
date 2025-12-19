namespace PlcAssistant.Domain.Entities;

/// <summary>
/// Represents a PLC memory address with its configuration and current value
/// </summary>
public class PlcMemoryAddress
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public PlcDataType DataType { get; set; }
    public string? CurrentValue { get; set; }
    public string? Description { get; set; }
    public bool IsReadOnly { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifiedAt { get; set; }
}
