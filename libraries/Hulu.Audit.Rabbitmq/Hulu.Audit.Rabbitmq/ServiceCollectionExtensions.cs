using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Hulu.Audit.Rabbitmq;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAuditRabbitMQ(this IServiceCollection services, RabbitMqConfiguration config)
    {
        var factory = new ConnectionFactory()
        {
            HostName = config.HostName,
            UserName = config.UserName,
            Password = config.Password,
            VirtualHost = config.VirtualHost
        };
        var connection = factory.CreateConnection();

        return services
            .AddSingleton<IConnection>(connection)
            .AddSingleton<IModel>(_ => 
            {
                var channel = connection.CreateModel();
                channel.QueueBind(config.QueueName, config.ExchangeName, config.RoutingKey);
                return channel;
            })
            .AddScoped<ILoggingService, LoggingService>();
    }
}
