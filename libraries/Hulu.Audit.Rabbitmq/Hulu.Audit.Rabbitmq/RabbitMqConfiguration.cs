namespace Hulu.Audit.Rabbitmq;

public class RabbitMqConfiguration
{
    public static string KeyName { get; } = "RabbitMQ";
    public string HostName { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string VirtualHost { get; init; } = string.Empty;
    public string ExchangeName { get; init; } = string.Empty;
    public string ExchangeType { get; init; } = string.Empty;
    public string QueueName { get; init; } = string.Empty;
    public string RoutingKey { get; init; } = string.Empty;
}