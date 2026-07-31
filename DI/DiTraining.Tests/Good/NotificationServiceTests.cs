using System;
using Xunit;
using DiTraining.Good;

namespace DiTraining.Tests.Good;

[Collection("Sequential")]
public class NotificationServiceTests
{
    private class FakeMessageSender : IMessageSender
    {
        public string? LastMessage { get; private set; }
        public void Send(string message) => LastMessage = message;
    }

    [Fact]
    public void Should_Send_Notification_Via_Injected_MessageSender()
    {
        // Arrange
        var fakeSender = new FakeMessageSender();
        var service = new NotificationService(fakeSender);

        // Act
        service.Notify("Hello Good");

        // Assert
        Assert.Equal("Hello Good", fakeSender.LastMessage);
    }
}
