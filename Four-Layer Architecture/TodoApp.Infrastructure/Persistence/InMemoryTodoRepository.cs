using System.Collections.Concurrent;
using TodoApp.Domain.Todos;

namespace TodoApp.Infrastructure.Persistence;

public sealed class InMemoryTodoRepository : ITodoRepository
{
    private readonly ConcurrentDictionary<Guid, TodoItem> store = new();

    public Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(store.GetValueOrDefault(id));
    public Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<TodoItem>>(store.Values.OrderBy(x => x.CreatedAt).ToList());
    public Task AddAsync(TodoItem entity, CancellationToken cancellationToken = default)
    {
        store[entity.Id] = entity;
        return Task.CompletedTask;
    }
    public Task UpdateAsync(TodoItem entity, CancellationToken cancellationToken = default)
    {
        store[entity.Id] = entity;
        return Task.CompletedTask;
    }
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        store.TryRemove(id, out _);
        return Task.CompletedTask;
    }
    public Task<bool> ExistsByTitleAsync(string title, Guid? excludingId = null, CancellationToken cancellationToken = default) =>
        Task.FromResult(store.Values.Any(x => x.Id != excludingId && string.Equals(x.Title, title.Trim(), StringComparison.OrdinalIgnoreCase)));
}
