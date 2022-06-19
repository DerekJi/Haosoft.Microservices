namespace Hulu.Audit.Rabbitmq;

public interface ILoggingService
{
    void Publish(string message);
}
