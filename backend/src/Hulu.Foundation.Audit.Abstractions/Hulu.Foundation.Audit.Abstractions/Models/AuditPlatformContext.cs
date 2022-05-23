namespace Hulu.Foundation.Audit.Abstractions.Models;

public class AuditPlatformContext
{
    public string Host { get; set; } = string.Empty;

    public string Pod { get; set; } = string.Empty;

    public string Container { get; set; } = string.Empty;

    public string Namespace { get; set; } = string.Empty;
}
