namespace Hulu.Audit.Abstractions;

public class NetworkContext
{
    public string? Protocol { get; set; }

    public string? Host { get; set; }

    public string? DomainName { get; set; }

    public string? IPAddress { get; set; }

    public HttpMethod? HttpMethod { get; set; }

    public string? PathOrUrl { get; set; }

    public string? UserId { get; set; }
}