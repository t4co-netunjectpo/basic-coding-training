using FluentAssertions;
using SolidSample.Good.Domain;
using Xunit;

namespace SolidSample.Tests;

public class SalesRecordTests
{
    [Fact]
    public void SalesRecord_ShouldStoreProductAndAmount()
    {
        // Act
        var record = new SalesRecord("Apple", 100m);

        // Assert
        record.Product.Should().Be("Apple");
        record.Amount.Should().Be(100m);
    }
}
