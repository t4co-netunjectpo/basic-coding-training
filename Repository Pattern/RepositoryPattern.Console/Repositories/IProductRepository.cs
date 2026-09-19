using RepositoryPattern.Console.Entities;

namespace RepositoryPattern.Console.Repositories;

public interface IProductRepository
{
    Product? GetById(int id);
    void DecreaseStock(int id, int quantity);
}
