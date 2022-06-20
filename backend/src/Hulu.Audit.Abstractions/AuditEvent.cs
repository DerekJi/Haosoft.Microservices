using System.Text.Json.Serialization;

namespace Hulu.Audit.Abstractions;

public class AuditEvent
{
    public AuditEvent()
    {
        Id = Guid.NewGuid();
        CreatedOn = DateTimeOffset.UtcNow;
        Severity = AuditSeverityLevel.Info;
    }

    public Guid? Id { get; init; }

    /// <summary>
    /// The date/time
    /// </summary>
    public DateTimeOffset? CreatedOn { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public AuditSeverityLevel Severity { get; set; }

    /// <summary>
    /// Human readable message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Json-format business data
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BusinessContext { get; set; }

    /// <summary>
    /// Mainly HTTP context
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NetworkContext? NetworkContext { get; set; }

    /// <summary>
    /// OS and Cluster context
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlatformContext? PlatformContext { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Exception? Exception { get; set; }
}
