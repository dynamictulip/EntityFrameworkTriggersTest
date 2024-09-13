using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkTriggersTest;

[PrimaryKey(nameof(Id))]
public class AuditEvent
{
    public Guid Id { get; }
    public string TableName { get; set; }

    public string PrimaryKeyColumnName { get; set; }
    public string PrimaryKeyType { get; set; }
    public object PrimaryKeyValueAsJson { get; set; }
    public EntityOperationType EntityOperationType { get; set; }
    public string AlteredColumnName { get; set; }

    public string AlteredColumnType { get; set; }

//    public object? OldValueAsJson { get; set; }
//    public object? NewValueAsJson { get; set; }
    public string? AuditMessage { get; set; }
}

public enum EntityOperationType
{
    Added = 1,
    Modified = 2,
    Removed = 3
}