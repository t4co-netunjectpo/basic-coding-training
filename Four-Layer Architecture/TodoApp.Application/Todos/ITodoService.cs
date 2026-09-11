namespace TodoApp.Application.Todos;

public interface ITodoService
{
    Task<TodoDto> CreateAsync(CreateTodoRequest request, CancellationToken cancellationToken = default);
    Task<TodoDto?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, UpdateTodoRequest request, CancellationToken cancellationToken = default);
    Task CompleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
