using System.Text;
using Hulu.Audit.Rabbitmq;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace FooHttp.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ILoggingService _service;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ILoggingService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("Get Weather Forecast ...");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost(Name = "Message")]
    public ActionResult PublishMessage(string message)
    {
        _service.Publish(message);

        return Ok();
    }
}
