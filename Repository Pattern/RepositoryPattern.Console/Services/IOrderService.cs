namespace RepositoryPattern.Console.Services;

public interface IOrderService
{
    int PlaceOrder(string customerName, int productId, int quantity);
}
