using Dapper;
using Microsoft.Data.Sqlite;

namespace RepositoryPattern.Console;

public static class DatabaseInitializer
{
    public static void Initialize(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        connection.Execute("""
            CREATE TABLE IF NOT EXISTS Products (
                Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Price INTEGER NOT NULL, Stock INTEGER NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Orders (
                Id INTEGER PRIMARY KEY AUTOINCREMENT, CustomerName TEXT NOT NULL, OrderedAt TEXT NOT NULL, TotalPrice INTEGER NOT NULL
            );
            CREATE TABLE IF NOT EXISTS OrderItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT, OrderId INTEGER NOT NULL, ProductId INTEGER NOT NULL,
                Quantity INTEGER NOT NULL, UnitPrice INTEGER NOT NULL,
                FOREIGN KEY (OrderId) REFERENCES Orders(Id), FOREIGN KEY (ProductId) REFERENCES Products(Id)
            );
            """);
        if (connection.ExecuteScalar<long>("SELECT COUNT(*) FROM Products;") == 0)
        {
            connection.Execute("""
                INSERT INTO Products (Id, Name, Price, Stock) VALUES
                (1, 'ノートPC', 120000, 5), (2, 'ワイヤレスマウス', 3000, 20), (3, 'USBメモリ', 1500, 0);
                """);
        }
    }
}
