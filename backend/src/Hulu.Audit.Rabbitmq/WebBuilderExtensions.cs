using Hulu.Audit.Abstractions;
using Microsoft.AspNetCore.Builder;
using RabbitMQ.Client;

namespace Hulu.Audit.Rabbitmq;

public static class WebBuilderExtensions
{
    public static WebApplicationBuilder UseAuditRabbitMQ(this WebApplicationBuilder builder)
    {
        //
        var services = builder.Services;
        var configurationManager = builder.Configuration;

        //
        services
            .UseLonglivedConnection(configurationManager)
            .AddScoped<IAuditService, AuditService>();

        return builder;
    }

    private static IServiceCollection UseLonglivedConnection(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        var config = new RabbitmqOptions();
        var configuration = configurationManager.GetSection(RabbitmqOptions.KeyName);
        configuration.Bind(config);
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
            });
    }
}
