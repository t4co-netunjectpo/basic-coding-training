using System.Collections.Generic;
using FluentAssertions;
using SolidSample.Good.Domain;
using SolidSample.Good.Formatters;
using Xunit;

namespace SolidSample.Tests;

public class ReportFormatterTests
{
    private readonly List<SalesRecord> _records = new()
    {
        new SalesRecord("Apple", 100m),
        new SalesRecord("Banana", 200m)
    };

    [Fact]
    public void CsvFormatter_ShouldFormatAsCsv()
    {
        // Arrange
        IReportFormatter formatter = new CsvFormatter();

        // Act
        var result = formatter.Format(_records);

        // Assert
        formatter.FormatName.Should().Be("CSV");
        result.Should().Contain("Product,Amount");
        result.Should().Contain("Apple,100");
        result.Should().Contain("Banana,200");
    }

    [Fact]
    public void JsonFormatter_ShouldFormatAsJson()
    {
        // Arrange
        IReportFormatter formatter = new JsonFormatter();

        // Act
        var result = formatter.Format(_records);

        // Assert
        formatter.FormatName.Should().Be("JSON");
        result.Should().Contain("Apple");
        result.Should().Contain("100");
        result.Should().Contain("Banana");
        result.Should().Contain("200");
    }

    [Fact]
    public void MarkdownFormatter_ShouldFormatAsMarkdownTable()
    {
        // Arrange
        IReportFormatter formatter = new MarkdownFormatter();

        // Act
        var result = formatter.Format(_records);

        // Assert
        formatter.FormatName.Should().Be("Markdown");
        result.Should().Contain("| Product | Amount |");
        result.Should().Contain("| Apple | 100 |");
        result.Should().Contain("| Banana | 200 |");
    }
}
