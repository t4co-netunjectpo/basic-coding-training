using RepositoryPattern.Console.Entities;

namespace RepositoryPattern.Console.Repositories;

public interface IOrderRepository
{
    int AddOrder(Order order);
    void AddOrderItem(OrderItem item);
}
