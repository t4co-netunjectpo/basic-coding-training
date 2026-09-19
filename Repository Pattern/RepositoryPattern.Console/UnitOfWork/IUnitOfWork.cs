using RepositoryPattern.Console.Repositories;

namespace RepositoryPattern.Console.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    void Commit();
    void Rollback();
}
