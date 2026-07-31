using System;
using System.IO;
using Xunit;
using DiTraining.Bad;

namespace DiTraining.Tests.Bad;

[Collection("Sequential")]
public class NotificationServiceTests
{
    [Fact]
    public void Should_Send_Notification_Via_EmailSender()
    {
        // Arrange
        var service = new NotificationService();
        var originalOut = Console.Out;
        using var sw = new StringWriter();
        Console.SetOut(sw);

        try
        {
            // Act
            service.Notify("Hello");

            // Assert
            var output = sw.ToString().Trim();
            Assert.Equal("[Email] Hello", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}
