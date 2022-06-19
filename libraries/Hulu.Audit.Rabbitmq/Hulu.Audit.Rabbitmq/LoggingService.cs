using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Hulu.Audit.Rabbitmq;

public class LoggingService : ILoggingService
{
    private readonly RabbitMqConfiguration _config;
    private readonly ILogger<LoggingService> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public LoggingService(IOptionsSnapshot<RabbitMqConfiguration> options, 
        IConnection connection,
        IModel channel,
        ILogger<LoggingService> logger)
    {
        _config = options.Value;
        _connection = connection;
        _channel = channel;
        _logger = logger;
    }

    public void Publish(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: _config.ExchangeName,
                                routingKey: _config.RoutingKey,
                                basicProperties: null,
                                body: body);

        _logger.LogDebug(" [x] Sent {0}", message);
    }
}
