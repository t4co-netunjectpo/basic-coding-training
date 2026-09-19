using Dapper;
using Microsoft.Data.Sqlite;

namespace RepositoryPattern.Console.Bad;

// Teaching anti-pattern: SQL, connection management, and business rules are all coupled here.
public sealed class DirectSqlOrderService(string connectionString)
{
    public int PlaceOrder(string customerName, int productId, int quantity)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var product = connection.QuerySingleOrDefault<(int Id, string Name, long Price, int Stock)>(
            "SELECT Id, Name, Price, Stock FROM Products WHERE Id = @productId;", new { productId });
        if (product.Id == 0)
            throw new InvalidOperationException($"商品ID {productId} は存在しません。");
        if (product.Stock < quantity)
            throw new InvalidOperationException($"商品「{product.Name}」の在庫が不足しています。");

        connection.Execute("UPDATE Products SET Stock = Stock - @quantity WHERE Id = @productId;",
            new { productId, quantity });
        var orderId = connection.ExecuteScalar<int>(
            "INSERT INTO Orders (CustomerName, OrderedAt, TotalPrice) VALUES (@customerName, @orderedAt, @totalPrice); SELECT last_insert_rowid();",
            new { customerName, orderedAt = DateTime.UtcNow, totalPrice = product.Price * quantity });
        connection.Execute("INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@orderId, @productId, @quantity, @unitPrice);",
            new { orderId, productId, quantity, unitPrice = product.Price });
        return orderId;
    }
}
