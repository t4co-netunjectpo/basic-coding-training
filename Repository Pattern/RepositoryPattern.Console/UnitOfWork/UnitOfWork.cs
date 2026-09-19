using Microsoft.Data.Sqlite;
using RepositoryPattern.Console.Repositories;

namespace RepositoryPattern.Console.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly SqliteConnection connection;
    private readonly SqliteTransaction transaction;
    private bool completed;

    public UnitOfWork(string connectionString, bool failOrderItemInsert = false)
    {
        connection = new SqliteConnection(connectionString);
        connection.Open();
        transaction = connection.BeginTransaction();
        Products = new ProductRepository(connection, transaction);
        Orders = new OrderRepository(connection, transaction, failOrderItemInsert);
    }

    public IProductRepository Products { get; }
    public IOrderRepository Orders { get; }

    public void Commit()
    {
        transaction.Commit();
        completed = true;
    }

    public void Rollback()
    {
        if (!completed)
            transaction.Rollback();
        completed = true;
    }

    public void Dispose()
    {
        if (!completed)
            Rollback();
        transaction.Dispose();
        connection.Dispose();
    }
}
