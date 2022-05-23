namespace Hulu.Foundation.Audit.Abstractions.Models;

public class AuditEvent
{
    public DateTimeOffset EventOn { get; set; }

    public AuditEventLevel Level { get; set; }

    public AuditNetworkContext NetworkContext { get; set; } = new();

    public AuditPlatformContext PlatformContext { get; set; } = new();

    /// <summary>
    /// A brief title of the message
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Indicates the catelog of the message
    /// </summary>
    public string? Cateloge { get; set; }

    /// <summary>
    /// Indicates the type of the message/event
    /// </summary>
    /// <example>request.summary, signin.failure</example>
    public string? Code { get; set; }

    /// <summary>
    /// Human readable string
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Custom data
    /// </summary>
    public string? JsonData { get; set; }
}
