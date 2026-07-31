using System;
using System.IO;
using Xunit;
using DiTraining.Bad;

namespace DiTraining.Tests.Bad;

[Collection("Sequential")]
public class EmailSenderTests
{
    [Fact]
    public void Should_Print_Email_Message_To_Console()
    {
        // Arrange
        var sender = new EmailSender();
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
}
