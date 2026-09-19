using System.Data;
using Dapper;
using RepositoryPattern.Console.Entities;

namespace RepositoryPattern.Console.Repositories;

public sealed class ProductRepository(IDbConnection connection, IDbTransaction transaction) : IProductRepository
{
    public Product? GetById(int id) =>
        connection.QuerySingleOrDefault<Product>(
            "SELECT Id, Name, Price, Stock FROM Products WHERE Id = @id;",
            new { id }, transaction);

    public void DecreaseStock(int id, int quantity)
    {
        var updated = connection.Execute(
            "UPDATE Products SET Stock = Stock - @quantity WHERE Id = @id AND Stock >= @quantity;",
            new { id, quantity }, transaction);
        if (updated != 1)
            throw new InvalidOperationException("在庫の更新に失敗しました。");
    }
}
