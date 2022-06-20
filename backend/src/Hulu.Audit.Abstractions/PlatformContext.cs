namespace Hulu.Audit.Abstractions;

public class PlatformContext
{
    public string? OS { get; set; }
    public string? Host { get; set; }
    public string? Context { get; set; }
    public string? Namespace { get; set; }
    public string? Service { get; set; }
    public string? Pod { get; set; }
    public string? Container { get; set; }
    public string? Network { get; set; }
}
