namespace TodoApp.Application.Todos;

public sealed record TodoDto(Guid Id, string Title, string? Description, bool IsCompleted, DateTime CreatedAt, DateTime UpdatedAt);
public sealed record CreateTodoRequest(string? Title, string? Description);
public sealed record UpdateTodoRequest(string? Title, string? Description);

public static class TodoRequestValidator
{
    public static Dictionary<string, string[]> Validate(string? title) =>
        string.IsNullOrWhiteSpace(title)
            ? new() { ["title"] = ["タイトルは必須です。"] }
            : title.Trim().Length > 100
                ? new() { ["title"] = ["タイトルは100文字以内で入力してください。"] }
                : [];
}
