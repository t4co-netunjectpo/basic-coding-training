namespace RepositoryPattern.Console.Entities;

public sealed class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long Price { get; set; }
    public int Stock { get; set; }
}
