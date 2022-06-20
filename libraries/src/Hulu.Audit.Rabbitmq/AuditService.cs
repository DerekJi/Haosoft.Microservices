using Hulu.Audit.Abstractions;
using Hulu.Audit.Abstractions.Extensions;
using RabbitMQ.Client;

namespace Hulu.Audit.Rabbitmq;

public class AuditService : IAuditService
{
    private readonly RabbitmqOptions _config;
    private readonly ILogger<AuditService> _logger;
    private readonly IConnection _connection;
    private IModel _channel { get; set; }

    public AuditService(IOptionsSnapshot<RabbitmqOptions> options,
        IConnection connection,
        IModel channel,
        ILogger<AuditService> logger)
    {
        _config = options.Value;
        _connection = connection;
        _channel = channel;
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Log(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentNullException(nameof(message), "must not be null or empty");
        }

        var builder = new AuditEventBuilder(message);
        Log(builder.GetInstnace());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="audit"></param>
    public void Log(AuditEvent auditEvent)
    {
        if (string.IsNullOrEmpty(auditEvent?.Message))
        {
            throw new ArgumentNullException("Message", "must not be null or empty");
        }

        if (_channel == null || _channel.IsClosed)
        {
            _channel = _connection.CreateModel();
        }

        var bytes = auditEvent.GetBytes();
        _channel.BasicPublish(exchange: _config.ExchangeName,
                            routingKey: _config.RoutingKey,
                            mandatory: true,
                            basicProperties: null,
                            body: bytes);

        _logger.LogDebug(" [x] Sent {0}", auditEvent.Message);
    }
}
