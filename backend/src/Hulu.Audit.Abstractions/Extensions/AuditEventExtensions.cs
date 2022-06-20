namespace Hulu.Audit.Abstractions.Extensions;

public static class AuditEventExtensions
{
    public static byte[] GetBytes(this AuditEvent? auditEvent)
    {
        if (auditEvent == null)
        {
            return new byte[0];
        }

        var message = JsonSerializer.Serialize(auditEvent);

        return Encoding.UTF8.GetBytes(message);
    }
}
