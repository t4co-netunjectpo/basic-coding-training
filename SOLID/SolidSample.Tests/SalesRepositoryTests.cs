using System.Collections.Generic;
using FluentAssertions;
using SolidSample.Good.DataAccess;
using SolidSample.Good.Domain;
using Xunit;

namespace SolidSample.Tests;

public class SalesRepositoryTests
{
    [Fact]
    public void InMemorySalesRepository_ShouldReturnExpectedSalesRecords()
    {
        // Arrange
        ISalesRepository repository = new InMemorySalesRepository();

        // Act
        IReadOnlyList<SalesRecord> records = repository.GetAll();

        // Assert
        records.Should().HaveCount(3);
        records[0].Product.Should().Be("Apple");
        records[0].Amount.Should().Be(100m);
        records[1].Product.Should().Be("Banana");
        records[1].Amount.Should().Be(200m);
        records[2].Product.Should().Be("Cherry");
        records[2].Amount.Should().Be(300m);
    }
}
