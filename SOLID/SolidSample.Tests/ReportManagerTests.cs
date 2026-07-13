using System;
using System.IO;
using FluentAssertions;
using SolidSample.Bad;
using Xunit;

namespace SolidSample.Tests;

public class ReportManagerTests
{
    [Fact]
    public void GenerateAndDeliverReport_ShouldOutputCsv_WhenFormatIsCsv()
    {
        // Arrange
        var manager = new ReportManager();
        using var sw = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(sw);

        try
        {
            // Act
            manager.GenerateAndDeliverReport("CSV", "Console", null);
            var output = sw.ToString();

            // Assert
            output.Should().Contain("Product,Amount");
            output.Should().Contain("Apple,100");
            output.Should().Contain("Banana,200");
            output.Should().Contain("Cherry,300");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void GenerateAndDeliverReport_ShouldOutputJson_WhenFormatIsJson()
    {
        // Arrange
        var manager = new ReportManager();
        using var sw = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(sw);

        try
        {
            // Act
            manager.GenerateAndDeliverReport("JSON", "Console", null);
            var output = sw.ToString();

            // Assert
            output.Should().Contain("Apple");
            output.Should().Contain("100");
            output.Should().Contain("Banana");
            output.Should().Contain("Cherry");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void GenerateAndDeliverReport_ShouldWriteToFile_WhenOutputTargetIsFile()
    {
        // Arrange
        var manager = new ReportManager();
        var tempFile = Path.Combine(Directory.GetCurrentDirectory(), "report.txt");
        if (File.Exists(tempFile))
        {
            File.Delete(tempFile);
        }

        try
        {
            // Act
            manager.GenerateAndDeliverReport("CSV", "File", null);

            // Assert
            File.Exists(tempFile).Should().BeTrue();
            var content = File.ReadAllText(tempFile);
            content.Should().Contain("Product,Amount");
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
    public void GenerateAndDeliverReport_ShouldSendEmail_WhenOutputTargetIsEmail()
    {
        // Arrange
        var manager = new ReportManager();
        using var sw = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(sw);

        try
        {
            // Act
            manager.GenerateAndDeliverReport("CSV", "Email", "test@example.com");
            var output = sw.ToString();

            // Assert
            output.Should().Contain("Email sent to test@example.com");
            output.Should().Contain("smtp.mymailserver.com"); // DIP violation check: hardcoded SMTP Server name
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void GenerateAndDeliverReport_ShouldThrowNotSupportedException_WhenPdfToConsole()
    {
        // Arrange
        var manager = new ReportManager();

        // Act
        var act = () => manager.GenerateAndDeliverReport("PDF", "Console", null);

        // Assert
        act.Should().Throw<NotSupportedException>().WithMessage("*PDF*");
    }
}
