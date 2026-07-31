using System;
using System.IO;
using Xunit;
using DiTraining.Good;

namespace DiTraining.Tests.Good;

[Collection("Sequential")]
public class MessageSenderTests
{
    [Fact]
    public void EmailSender_Should_Print_Email_Message_To_Console()
    {
        // Arrange
        IMessageSender sender = new EmailSender();
        var originalOut = Console.Out;
        using var sw = new StringWriter();
        Console.SetOut(sw);

        try
        {
            // Act
            sender.Send("Hello");

            // Assert
            var output = sw.ToString().Trim();
            Assert.Equal("[Email] Hello", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void SmsSender_Should_Print_Sms_Message_To_Console()
    {
        // Arrange
        IMessageSender sender = new SmsSender();
        var originalOut = Console.Out;
        using var sw = new StringWriter();
        Console.SetOut(sw);

        try
        {
            // Act
            sender.Send("Hello");

            // Assert
            var output = sw.ToString().Trim();
            Assert.Equal("[SMS] Hello", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}
