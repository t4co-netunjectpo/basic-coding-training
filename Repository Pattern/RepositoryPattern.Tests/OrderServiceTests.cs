using RepositoryPattern.Console.Entities;
using RepositoryPattern.Console.Repositories;
using RepositoryPattern.Console.Services;
using RepositoryPattern.Console.UnitOfWork;

namespace RepositoryPattern.Tests;

public sealed class OrderServiceTests
{
    [Fact]
    public void PlaceOrder_CommitsAndPersistsOrder()
    {
        var uow = new FakeUnitOfWork(new Product { Id = 1, Name = "商品", Price = 10, Stock = 3 });
        var service = new OrderService(() => uow);

        var id = service.PlaceOrder("顧客", 1, 2);

        Assert.Equal(42, id);
        Assert.Equal(1, uow.CommitCount);
        Assert.Equal(0, uow.RollbackCount);
        Assert.Single(uow.OrderItems);
        Assert.Equal(1, uow.ProductsFake.Product!.Stock);
    }

    [Fact]
    public void PlaceOrder_RollsBackWhenRepositoryThrows_AndPropagatesException()
    {
        var uow = new FakeUnitOfWork(new Product { Id = 1, Name = "商品", Price = 10, Stock = 3 })
        {
            ThrowOnOrderItem = true
        };
        var service = new OrderService(() => uow);

        var exception = Assert.Throws<InvalidOperationException>(() => service.PlaceOrder("顧客", 1, 1));

        Assert.Equal("fake insert failure", exception.Message);
        Assert.Equal(0, uow.CommitCount);
        Assert.Equal(1, uow.RollbackCount);
    }

    [Fact]
    public void PlaceOrder_RejectsInsufficientStockWithoutChangingData()
    {
        var uow = new FakeUnitOfWork(new Product { Id = 1, Name = "商品", Price = 10, Stock = 1 });
        var service = new OrderService(() => uow);

        Assert.Throws<InvalidOperationException>(() => service.PlaceOrder("顧客", 1, 2));

        Assert.Equal(1, uow.RollbackCount);
        Assert.Equal(0, uow.CommitCount);
        Assert.Equal(1, uow.ProductsFake.Product!.Stock);
        Assert.Empty(uow.OrderItems);
    }

    [Fact]
    public void PlaceOrder_RejectsUnknownProduct()
    {
        var uow = new FakeUnitOfWork(null);
        var service = new OrderService(() => uow);

        Assert.Throws<InvalidOperationException>(() => service.PlaceOrder("顧客", 999, 1));
        Assert.Equal(1, uow.RollbackCount);
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public FakeProductRepository ProductsFake { get; }
        private FakeOrderRepository OrdersFake { get; } = new();
        public bool ThrowOnOrderItem { get; set; }
        public int CommitCount { get; private set; }
        public int RollbackCount { get; private set; }
        public List<OrderItem> OrderItems => OrdersFake.Items;
        public IProductRepository Products => ProductsFake;
        public IOrderRepository Orders => OrdersFake;
        public void Commit() => CommitCount++;
        public void Rollback() => RollbackCount++;
        public void Dispose() { }
        public FakeUnitOfWork(Product? product)
        {
            ProductsFake = new FakeProductRepository(product);
            OrdersFake.ThrowOnOrderItemOwner = this;
        }
        private sealed class FakeOrderRepository : IOrderRepository
        {
            public List<OrderItem> Items { get; } = [];
            public int AddOrder(Order order) => 42;
            public void AddOrderItem(OrderItem item)
            {
                if (item.ProductId == 1 && item.Quantity == 1 && ThrowOnOrderItemOwner?.ThrowOnOrderItem == true)
                    throw new InvalidOperationException("fake insert failure");
                Items.Add(item);
            }
            public FakeUnitOfWork? ThrowOnOrderItemOwner { get; set; }
        }
    }

    private sealed class FakeProductRepository(Product? product) : IProductRepository
    {
        public Product? Product { get; private set; } = product;
        public Product? GetById(int id) => Product?.Id == id ? Product : null;
        public void DecreaseStock(int id, int quantity) => Product!.Stock -= quantity;
    }
}
