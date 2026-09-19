using Dapper;
using Microsoft.Data.Sqlite;
using RepositoryPattern.Console;
using RepositoryPattern.Console.Bad;
using RepositoryPattern.Console.Services;
using RepositoryPattern.Console.UnitOfWork;

var connectionString = "Data Source=shop.db";
DatabaseInitializer.Initialize(connectionString);

static void Run(string title, Action action)
{
    Console.WriteLine($"\n--- {title} ---");
    try
    {
        action();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"失敗: {ex.Message}");
    }
}

var good = new OrderService(() => new UnitOfWork(connectionString));
Run("Good: 成功", () =>
{
    var orderId = good.PlaceOrder("田中太郎", 1, 2);
    Console.WriteLine($"注文 {orderId} を確定しました。");
});
Run("Good: 在庫不足", () => good.PlaceOrder("佐藤花子", 3, 1));
Run("Good: 商品不存在", () => good.PlaceOrder("鈴木一郎", 999, 1));
Run("Good: 明細登録失敗とRollback", () =>
{
    var rollbackService = new OrderService(() => new UnitOfWork(connectionString, failOrderItemInsert: true));
    rollbackService.PlaceOrder("高橋次郎", 2, 1);
});
Run("Bad: 直接SQL（比較用アンチパターン）", () =>
{
    var orderId = new DirectSqlOrderService(connectionString).PlaceOrder("山田三郎", 2, 1);
    Console.WriteLine($"注文 {orderId} を確定しました。");
});

using var check = new SqliteConnection(connectionString);
Console.WriteLine("\n現在の商品在庫:");
foreach (var product in check.Query("SELECT Id, Name, Stock FROM Products ORDER BY Id"))
    Console.WriteLine($"  {product.Id}: {product.Name} = {product.Stock}");
