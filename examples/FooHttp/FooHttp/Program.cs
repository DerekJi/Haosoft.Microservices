using Hulu.Audit.Rabbitmq;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseSerilog();
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Hulu Services
var rabbitMqConfigs = new RabbitMqConfiguration();
var configuration = builder.Configuration.GetSection(RabbitMqConfiguration.KeyName);
configuration.Bind(rabbitMqConfigs);
builder.Services.Configure<RabbitMqConfiguration>(configuration);
builder.Services.UseAuditRabbitMQ(rabbitMqConfigs);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
