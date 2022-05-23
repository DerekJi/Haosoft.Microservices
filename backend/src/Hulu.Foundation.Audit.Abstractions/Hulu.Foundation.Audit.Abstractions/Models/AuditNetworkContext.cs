namespace Hulu.Foundation.Audit.Abstractions.Models;

public class AuditNetworkContext
{
    /// <summary>
    /// The host name
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// The port number on the host
    /// </summary>
    /// <example>80, 443, 1521, 5432</example>
    public string Port { get; set; } = string.Empty;

    /// <summary>
    /// The network protocol 
    /// </summary>
    /// <example>Http, Https, gRpc, Wcf, ... etc</example>
    public string Protocol { get; set; } = string.Empty;

    /// <summary>
    /// Typically the Http Method
    /// </summary>
    /// <example>GET, POST, PUT, DELETE</example>
    public string Method { get; set; } = string.Empty;

    /// <summary>
    /// The remote address of the host. Typically it's the IP address
    /// </summary>
    public string RemoteAddress { get; set; } = string.Empty;
}
