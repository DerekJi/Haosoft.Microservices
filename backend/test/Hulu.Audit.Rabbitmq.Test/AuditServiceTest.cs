using Hulu.Audit.Abstractions;

namespace Hulu.Audit.Rabbitmq;

public class AuditServiceTest
{
    private readonly ILogger<AuditService> _logger = new NullLogger<AuditService>();
    private readonly Mock<IOptionsSnapshot<RabbitmqOptions>> _options = new();
    private readonly Mock<IConnection> _connection = new();
    private readonly Mock<IModel> _channel1 = new();
    private readonly Mock<IModel> _channel2 = new();
    private readonly AuditEvent _event;

    private readonly string _message = "some-message";
    private readonly string[] _nullOrEmptyValues = new string[] { "", null, string.Empty };
    private readonly RabbitmqOptions _config = new RabbitmqOptions()
    {
        QueueName = "some-queue",
        ExchangeName = "some-exchange",
        RoutingKey = "some-routing-key",
    };

    public AuditServiceTest()
    {
        _event = new AuditEventBuilder(_message).GetInstnace();
        _options.Setup(m => m.Value).Returns(_config);
        _connection.Setup(x => x.CreateModel()).Returns(_channel2.Object);
    }

    private IAuditService CreateServiceInstance(IModel channel)
    {
        return new AuditService(_options.Object, _connection.Object, channel, _logger);
    }

    #region Log(AuditEvent)
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void LogAuditEvent_NullOrEmptyMessage_ShouldThrowArguementError(int i)
    {
        // Arrnage
        var service = CreateServiceInstance(_channel1.Object);
        _event.Message = _nullOrEmptyValues[i];

        // Act
        var act = () => service.Log(_event);

        // Assert
        act.Should().Throw<ArgumentNullException>();

        _connection.Verify(x => x.CreateModel(), Times.Never, "because it should NOT be reached");
        _channel1.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Never,
                                $"because empty messages should NOT be published");
        _channel2.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Never,
                                $"because empty messages should NOT be published");
    }

    [Fact]
    public void LogAuditEvent_WithChannel1_ShouldPublishViaChannel1()
    {
        // Arrnage
        _channel1.Setup(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()));
        var service = CreateServiceInstance(_channel1.Object);

        // Act
        var act = () => service.Log(_event);

        // Assert
        act.Should().NotThrow<Exception>();
        _connection.Verify(x => x.CreateModel(), Times.Never, "because channel is still available");
        _channel1.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Once,
                                $"because the message should be published via {nameof(_channel1)}");
        _channel2.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Never,
                                $"because the message should NOT be published via {nameof(_channel2)}");
    }

    [Fact]
    public void LogAuditEvent_WithClosedChannel1_ShouldPublishViaChannel2()
    {
        // Arrange
        _channel1.Setup(x => x.IsClosed).Returns(true);
        _channel2.Setup(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()));
        var service = CreateServiceInstance(_channel1.Object);

        // Act
        var act = () => service.Log(_event);

        // Assert
        act.Should().NotThrow<Exception>();
        _connection.Verify(x => x.CreateModel(), Times.Once, "because channel was closed");
        _channel1.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, true, null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Never,
                                $"because the message should NOT be published via {nameof(_channel1)}");
        _channel2.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, true, null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Once,
                                $"because the message should be published from {nameof(_channel2)}");
    }

    [Fact]
    public void LogAuditEvent_WithNullChannel1_ShouldPublishViaChannel2()
    {
        // Arrange
        _channel2.Setup(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()));
        _channel2.Setup(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()));
        var service = CreateServiceInstance(null);

        // Act
        var act = () => service.Log(_event);

        // Assert
        act.Should().NotThrow<Exception>();
        _connection.Verify(x => x.CreateModel(), Times.Once, "because channel was closed");
        _channel1.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, true, null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Never,
                                $"because the message should NOT be published via {nameof(_channel1)}");
        _channel2.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, true, null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Once,
                                $"because the message should be published from {nameof(_channel2)}");
    }
    #endregion Log(AuditEvent)

    #region Log(string)
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void LogMessage_NullOrEmptyMessage_ShouldThrowArguementError(int i)
    {
        // Arrnage
        var service = CreateServiceInstance(_channel1.Object);
        var message = _nullOrEmptyValues[i];

        // Act
        var act = () => service.Log(message);

        // Assert
        act.Should().Throw<ArgumentNullException>();

        _connection.Verify(x => x.CreateModel(), Times.Never, "because it should NOT be reached");
        _channel1.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Never,
                                $"because empty messages should NOT be published");
        _channel2.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Never,
                                $"because empty messages should NOT be published");
    }

    [Fact]
    public void LogMessage_WithChannel1_ShouldPublishViaChannel1()
    {
        // Arrnage
        _channel1.Setup(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()));
        var service = CreateServiceInstance(_channel1.Object);

        // Act
        var act = () => service.Log(_message);

        // Assert
        act.Should().NotThrow<Exception>();
        _connection.Verify(x => x.CreateModel(), Times.Never, "because channel is still available");
        _channel1.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Once,
                                $"because the message should be published via {nameof(_channel1)}");
        _channel2.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Never,
                                $"because the message should NOT be published via {nameof(_channel2)}");
    }

    [Fact]
    public void LogMessage_WithClosedChannel1_ShouldPublishViaChannel2()
    {
        // Arrange
        _channel1.Setup(x => x.IsClosed).Returns(true);
        _channel2.Setup(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()));
        var service = CreateServiceInstance(_channel1.Object);

        // Act
        var act = () => service.Log(_message);

        // Assert
        act.Should().NotThrow<Exception>();
        _connection.Verify(x => x.CreateModel(), Times.Once, "because channel was closed");
        _channel1.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, true, null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Never,
                                $"because the message should NOT be published via {nameof(_channel1)}");
        _channel2.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, true, null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Once,
                                $"because the message should be published from {nameof(_channel2)}");
    }

    [Fact]
    public void LogMessage_WithNullChannel1_ShouldPublishViaChannel2()
    {
        // Arrange
        _channel2.Setup(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()));
        _channel2.Setup(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, It.IsAny<bool>(), null, It.IsAny<ReadOnlyMemory<byte>>()));
        var service = CreateServiceInstance(null);

        // Act
        var act = () => service.Log(_message);

        // Assert
        act.Should().NotThrow<Exception>();
        _connection.Verify(x => x.CreateModel(), Times.Once, "because channel was closed");
        _channel1.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, true, null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Never,
                                $"because the message should NOT be published via {nameof(_channel1)}");
        _channel2.Verify(x => x.BasicPublish(_config.ExchangeName, _config.RoutingKey, true, null, It.IsAny<ReadOnlyMemory<byte>>()),
                                Times.Once,
                                $"because the message should be published from {nameof(_channel2)}");
    }
    #endregion Log(string)
}