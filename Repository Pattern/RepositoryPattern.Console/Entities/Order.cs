namespace RepositoryPattern.Console.Entities;

public sealed class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderedAt { get; set; }
    public long TotalPrice { get; set; }
}
