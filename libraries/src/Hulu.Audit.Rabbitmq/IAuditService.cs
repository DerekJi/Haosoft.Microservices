using Hulu.Audit.Abstractions;

namespace Hulu.Audit.Rabbitmq;

public interface IAuditService
{
    void Log(AuditEvent aduitEvent);        

    void Log(string message);
}
