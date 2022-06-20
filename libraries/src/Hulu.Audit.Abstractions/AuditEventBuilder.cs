namespace Hulu.Audit.Abstractions;

public class AuditEventBuilder
{
    private AuditEvent _event { get; set; }

    public AuditEventBuilder()
    {
        _event = new AuditEvent();
    }

    public AuditEventBuilder(string message) : this()
    {
        _event.Message = message;
    }

    public AuditEventBuilder(string message, AuditSeverityLevel severity) : this(message)
    {
        _event.Message = message;
        _event.Severity = severity;
    }

    public AuditEvent GetInstnace()
    {
        return _event;
    }

    public AuditEventBuilder SetSeverityLevel(AuditSeverityLevel severity)
    {
        _event.Severity = severity;
        return this;
    }

    public AuditEventBuilder SetNetworkContext(NetworkContext context)
    {
        _event.NetworkContext = context;
        return this;
    }

    public AuditEventBuilder SetBusinessContext(string context)
    {
        _event.BusinessContext = context;
        return this;
    }

    public AuditEventBuilder SetBusinessContext(object context)
    {
        _event.BusinessContext = JsonSerializer.Serialize(context);
        return this;
    }

    public AuditEventBuilder SetException(Exception exception)
    {
        _event.Exception = exception;
        return this;
    }

    public AuditEventBuilder SetPlatformContext(PlatformContext context)
    {
        _event.PlatformContext = context;
        return this;
    }
}
