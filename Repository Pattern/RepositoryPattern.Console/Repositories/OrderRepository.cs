using System.Data;
using Dapper;
using RepositoryPattern.Console.Entities;

namespace RepositoryPattern.Console.Repositories;

public sealed class OrderRepository(IDbConnection connection, IDbTransaction transaction, bool failOrderItemInsert = false) : IOrderRepository
{
    public int AddOrder(Order order) =>
        connection.ExecuteScalar<int>(
            """
            INSERT INTO Orders (CustomerName, OrderedAt, TotalPrice)
            VALUES (@CustomerName, @OrderedAt, @TotalPrice);
            SELECT last_insert_rowid();
            """, order, transaction);

    public void AddOrderItem(OrderItem item)
    {
        if (failOrderItemInsert)
            throw new InvalidOperationException("デモ: 注文明細の登録に失敗しました。");

        connection.Execute(
            "INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice);",
            item, transaction);
    }
}
