using RepositoryPattern.Console.Entities;
using RepositoryPattern.Console.UnitOfWork;

namespace RepositoryPattern.Console.Services;

public sealed class OrderService(Func<IUnitOfWork> unitOfWorkFactory) : IOrderService
{
    public int PlaceOrder(string customerName, int productId, int quantity)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("顧客名は必須です。", nameof(customerName));
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "数量は1以上で指定してください。");

        using var unitOfWork = unitOfWorkFactory();
        try
        {
            var product = unitOfWork.Products.GetById(productId)
                ?? throw new InvalidOperationException($"商品ID {productId} は存在しません。");
            if (product.Stock < quantity)
                throw new InvalidOperationException($"商品「{product.Name}」の在庫が不足しています。");

            unitOfWork.Products.DecreaseStock(productId, quantity);
            var order = new Order
            {
                CustomerName = customerName,
                OrderedAt = DateTime.UtcNow,
                TotalPrice = product.Price * quantity
            };
            var orderId = unitOfWork.Orders.AddOrder(order);
            unitOfWork.Orders.AddOrderItem(new OrderItem
            {
                OrderId = orderId,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = product.Price
            });
            unitOfWork.Commit();
            return orderId;
        }
        catch
        {
            unitOfWork.Rollback();
            throw;
        }
    }
}
