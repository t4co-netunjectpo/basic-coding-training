using TodoApp.Domain.Common;

namespace TodoApp.Domain.Todos;

public interface ITodoRepository : IRepository<TodoItem>
{
    Task<bool> ExistsByTitleAsync(string title, Guid? excludingId = null, CancellationToken cancellationToken = default);
}
