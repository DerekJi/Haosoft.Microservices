using Hulu.Audit.Abstractions;
using Microsoft.AspNetCore.Builder;

namespace Hulu.Audit.Rabbitmq;

public static class WebBuilderExtensions
{
    public static WebApplicationBuilder UseRabbitmqOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RabbitmqOptions>(builder.Configuration.GetSection(RabbitmqOptions.KeyName));

        return builder;
    }
}
