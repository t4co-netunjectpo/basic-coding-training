using TodoApp.Domain.Todos;

namespace TodoApp.Application.Todos;

public sealed class TodoService(ITodoRepository repository) : ITodoService
{
    public async Task<TodoDto> CreateAsync(CreateTodoRequest request, CancellationToken cancellationToken = default)
    {
        var todo = new TodoItem(request.Title!, request.Description);
        if (await repository.ExistsByTitleAsync(todo.Title, cancellationToken: cancellationToken))
            throw new InvalidOperationException("同じタイトルのタスクが既に存在します。");
        await repository.AddAsync(todo, cancellationToken);
        return ToDto(todo);
    }

    public async Task<TodoDto?> GetAsync(Guid id, CancellationToken cancellationToken = default) =>
        (await repository.GetByIdAsync(id, cancellationToken)) is { } todo ? ToDto(todo) : null;

    public async Task<IReadOnlyList<TodoDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await repository.GetAllAsync(cancellationToken)).Select(ToDto).ToList();

    public async Task UpdateAsync(Guid id, UpdateTodoRequest request, CancellationToken cancellationToken = default)
    {
        var todo = await GetOrThrowAsync(id, cancellationToken);
        if (await repository.ExistsByTitleAsync(request.Title!, id, cancellationToken))
            throw new InvalidOperationException("同じタイトルのタスクが既に存在します。");
        todo.ChangeTitle(request.Title!);
        todo.ChangeDescription(request.Description);
        await repository.UpdateAsync(todo, cancellationToken);
    }

    public async Task CompleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var todo = await GetOrThrowAsync(id, cancellationToken);
        todo.Complete();
        await repository.UpdateAsync(todo, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _ = await GetOrThrowAsync(id, cancellationToken);
        await repository.DeleteAsync(id, cancellationToken);
    }

    private async Task<TodoItem> GetOrThrowAsync(Guid id, CancellationToken cancellationToken) =>
        await repository.GetByIdAsync(id, cancellationToken)
        ?? throw new KeyNotFoundException("指定されたタスクが見つかりません。");

    private static TodoDto ToDto(TodoItem item) => new(item.Id, item.Title, item.Description, item.IsCompleted, item.CreatedAt, item.UpdatedAt);
}
