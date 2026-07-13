using System;
using System.IO;
using FluentAssertions;
using SolidSample.Good.Output;
using Xunit;

namespace SolidSample.Tests;

public class OutputTests
{
    [Fact]
    public void FileWriter_ShouldWriteContentToFile()
    {
        // Arrange
        IFileWriter writer = new FileWriter();
        var tempFile = Path.Combine(Directory.GetCurrentDirectory(), "good_report.txt");
        if (File.Exists(tempFile))
        {
            File.Delete(tempFile);
        }

        try
        {
            // Act
            writer.Write(tempFile, "Hello Good SOLID");

            // Assert
            File.Exists(tempFile).Should().BeTrue();
            File.ReadAllText(tempFile).Should().Be("Hello Good SOLID");
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    [Fact]
    public void SmtpEmailSender_ShouldMockEmailDeliveryWithoutErrors()
    {
        // Arrange
        IEmailSender sender = new SmtpEmailSender();
        using var sw = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(sw);

        try
        {
            // Act
            sender.Send("user@example.com", "Test Subject", "Body Content");

            // Assert
            var output = sw.ToString();
            output.Should().Contain("Email sent to user@example.com");
            output.Should().Contain("Test Subject");
            output.Should().Contain("Body Content");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void ConsoleWriter_ShouldWriteToConsole()
    {
        // Arrange
        IConsoleWriter writer = new ConsoleWriter();
        using var sw = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(sw);

        try
        {
            // Act
            writer.Write("Console Output Test");

            // Assert
            var output = sw.ToString();
            output.Should().Be("Console Output Test");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}
