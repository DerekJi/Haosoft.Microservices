using Hulu.Audit.Abstractions.Extensions;

namespace Hulu.Audit.Abstractions.Test.Extensions;

public class AuditEventExtensionsTest
{
    [Fact]
    public void GetBytes_Null_ShouldReturnEmpty()
    {
        // Arrange
        AuditEvent? auditEvent = null;

        // Act
        var bytes = auditEvent.GetBytes();

        // Assert
        bytes.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void GetBytes_NotNull_ShouldReturnNotEmpty()
    {
        // Arrange
        AuditEvent? auditEvent = new AuditEventBuilder("some-message").GetInstnace();

        // Act
        var bytes = auditEvent.GetBytes();

        // Assert
        bytes.Should().NotBeNullOrEmpty();
    }
}
