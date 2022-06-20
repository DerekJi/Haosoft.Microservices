using Hulu.Audit.Abstractions;
using Hulu.Audit.Rabbitmq;
using Microsoft.AspNetCore.Mvc;

namespace FooHttp.Controllers;
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class FooController : ControllerBase
{
    private readonly ILogger<FooController> _logger;
    private readonly IAuditService _service;

    public FooController(ILogger<FooController> logger, IAuditService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost("Audit/Message")]
    public ActionResult PublishMessage(string message)
    {
        _service.Log(message);

        return Ok();
    }

    [HttpPost("Audit/Event")]
    public ActionResult PublishMessage([FromBody] AuditEvent aduitEvent)
    {
        _service.Log(aduitEvent);

        return Ok();
    }
}
