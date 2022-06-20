namespace Hulu.Audit.Abstractions.Test;

public class AuditEventBuilderTest
{
    [Fact]
    public void Constructor_WithNoMessageNoSeverity_ShouldCreateDefaultInstance()
    {
        // Arrange
        var expected = new
        {
            CreatedOn = DateTimeOffset.UtcNow,
            Severity = AuditSeverityLevel.Info,
        };

        // Act
        var builder = new AuditEventBuilder();
        var instance = builder.GetInstnace();

        // Assert
        builder.Should().NotBeNull();
        instance.Should().NotBeNull();
        instance.Id.Should().NotBeNull().And.NotBeEmpty();
        instance.Severity.Should().Be(expected.Severity, "because 'Severity' should be the default value");
        instance.CreatedOn.Should().NotBeNull().And.NotBeBefore(expected.CreatedOn);
        instance.Message.Should().BeNullOrEmpty("because instance should not contain a message");
    }

    [Fact]
    public void Constructor_WithMessageOnly_ShouldCreateInstanceWithMessage()
    {
        // Arrange
        var expected = new
        {
            Message = "some-expected-message",
            CreatedOn = DateTimeOffset.UtcNow,
            Severity = AuditSeverityLevel.Info,
        };

        // Act
        var builder = new AuditEventBuilder(expected.Message);
        var instance = builder.GetInstnace();

        // Assert
        builder.Should().NotBeNull();
        instance.Should().NotBeNull();
        instance.Id.Should().NotBeNull().And.NotBeEmpty();
        instance.Severity.Should().Be(expected.Severity, "because 'Severity' should be the default value");
        instance.CreatedOn.Should().NotBeNull().And.NotBeBefore(expected.CreatedOn);
        instance.Message.Should().NotBeNullOrEmpty().And.Be(expected.Message, "because instance should contain 'Message'");
    }

    [Theory]
    [InlineData(AuditSeverityLevel.Debug)]
    [InlineData(AuditSeverityLevel.Info)]
    [InlineData(AuditSeverityLevel.Warning)]
    [InlineData(AuditSeverityLevel.Error)]
    [InlineData(AuditSeverityLevel.Critical)]
    [InlineData(AuditSeverityLevel.Fatal)]
    public void Constructor_WithMessageAndSeverity_ShouldCreateInstanceWithMessageAndSeverity(AuditSeverityLevel severity)
    {
        // Arrange
        var expected = new
        {
            Message = "some-expected-message",
            CreatedOn = DateTimeOffset.UtcNow,
            Severity = severity,
        };

        // Act
        var builder = new AuditEventBuilder(expected.Message, expected.Severity);
        var instance = builder.GetInstnace();

        // Assert
        builder.Should().NotBeNull();
        instance.Should().NotBeNull();
        instance.Id.Should().NotBeNull().And.NotBeEmpty();
        instance.CreatedOn.Should().NotBeNull().And.NotBeBefore(expected.CreatedOn);
        instance.Message.Should().NotBeNullOrEmpty().And.Be(expected.Message, "because instance should contain 'Message'");
        instance.Severity.Should().Be(expected.Severity, "because 'Severity' should be the given value");
    }

    [Theory]
    [InlineData(AuditSeverityLevel.Debug)]
    [InlineData(AuditSeverityLevel.Info)]
    [InlineData(AuditSeverityLevel.Warning)]
    [InlineData(AuditSeverityLevel.Error)]
    [InlineData(AuditSeverityLevel.Critical)]
    [InlineData(AuditSeverityLevel.Fatal)]
    public void SetSeverityLevel_WithValue_ShouldSetSeverity(AuditSeverityLevel severity)
    {
        // Arrange
        var builder = new AuditEventBuilder();

        // Act
        var instance = builder.SetSeverityLevel(severity)
            .GetInstnace();

        // Assert
        instance.Severity.Should().Be(severity);
    }

    [Fact]
    public void SetNetworkContext_WithValue_ShouldSetNetworkContext()
    {
        // Arrange
        var builder = new AuditEventBuilder();
        var context = new NetworkContext()
        {
            Protocol = "some-protocol",
            Host = "localhost",
            DomainName = "devtest",
            IPAddress = "127.0.0.1",
            HttpMethod = HttpMethod.Get,
            PathOrUrl = "https://devtest.com/foo/bar",
            UserId = "some-user-id"
        };

        // Act
        var instance = builder.SetNetworkContext(context)
            .GetInstnace();

        // Assert - default
        instance.PlatformContext.Should().BeNull();
        instance.Exception.Should().BeNull();
        instance.BusinessContext.Should().BeNullOrEmpty();

        // Assert - NetworkContext
        instance.NetworkContext.Should().NotBeNull().And.BeEquivalentTo(context, "because NetworkContext should be the assigned value");
    }

    [Fact]
    public void SetBusinessContext_WithStringValue_ShouldSetBusinessContext()
    {
        // Arrange
        var builder = new AuditEventBuilder();
        var context = "some-business-context";

        // Act
        var instance = builder.SetBusinessContext(context)
            .GetInstnace();

        // Assert - default
        instance.PlatformContext.Should().BeNull();
        instance.Exception.Should().BeNull();
        instance.NetworkContext.Should().BeNull();

        // Assert - BusinessContext
        instance.BusinessContext.Should().Be(context);
    }

    [Fact]
    public void SetBusinessContext_WithObjectValue_ShouldSetBusinessContext()
    {
        // Arrange
        var builder = new AuditEventBuilder();
        var obj = new
        {
            Name = "some-name",
            Value = "some-value",
            Data = new
            {
                D1 = "some-data-1",
                D2 = "some-data-2",
            }
        };
        var context = JsonSerializer.Serialize(obj);

        // Act
        var instance = builder.SetBusinessContext(obj)
            .GetInstnace();

        // Assert - default
        instance.PlatformContext.Should().BeNull();
        instance.Exception.Should().BeNull();
        instance.NetworkContext.Should().BeNull();

        // Assert - BusinessContext
        instance.BusinessContext.Should().Be(context);
    }

    [Fact]
    public void SetException_WithNoInnerException_ShouldSetException()
    {
        // Arrange
        var builder = new AuditEventBuilder();
        var exception = new Exception("some-exception-message");

        // Act
        var instance = builder.SetException(exception)
            .GetInstnace();

        // Assert - default
        instance.PlatformContext.Should().BeNull();
        instance.NetworkContext.Should().BeNull();
        instance.BusinessContext.Should().BeNullOrEmpty();

        // Assert - Exception
        instance.Exception.Should().NotBeNull().And.BeEquivalentTo(exception);
    }

    [Fact]
    public void SetException_WithInnerException_ShouldSetException()
    {
        // Arrange
        var builder = new AuditEventBuilder();
        var innerEx = new NotImplementedException("Some Not Implemented Exception");
        var exception = new Exception("some-exception-message", innerEx);

        // Act
        var instance = builder.SetException(exception)
            .GetInstnace();

        // Assert - default
        instance.PlatformContext.Should().BeNull();
        instance.NetworkContext.Should().BeNull();
        instance.BusinessContext.Should().BeNullOrEmpty();

        // Assert - Exception
        instance.Exception.Should().NotBeNull().And.BeEquivalentTo(exception);
    }

    [Fact]
    public void SetPlatformContext_WithValue_ShouldSetPlatformContext()
    {
        // Arrange
        var context = new PlatformContext
        {
            OS = "Windows 10 Pro",
            Host = "LAPTOP-AABBCC",
            Context = "some-cluster-context-name",
            Namespace = "some-cluster-namespace",
            Service = "some-cluster-service",
            Pod = "some-cluster-pod",
            Container = "some-container",
            Network = "hulu-devtest-network",
        };
        var builder = new AuditEventBuilder();

        // Act
        var instance = builder.SetPlatformContext(context)
            .GetInstnace();

        // Assert - default
        instance.NetworkContext.Should().BeNull();
        instance.BusinessContext.Should().BeNullOrEmpty();
        instance.Exception.Should().BeNull();

        // Assert - PlatformContext
        instance.PlatformContext.Should().BeEquivalentTo(context);
    }
}