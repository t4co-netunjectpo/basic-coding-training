using TodoApp.Domain.Common;

namespace TodoApp.Domain.Todos;

public sealed class TodoItem : EntityBase
{
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public bool IsCompleted { get; private set; }

    public TodoItem(string title, string? description = null)
    {
        ValidateTitle(title);
        Title = title.Trim();
        Description = description;
    }

    public void ChangeTitle(string title)
    {
        ValidateTitle(title);
        Title = title.Trim();
        MarkUpdated();
    }

    public void ChangeDescription(string? description)
    {
        Description = description;
        MarkUpdated();
    }

    public void Complete()
    {
        if (IsCompleted)
            throw new InvalidOperationException("このタスクは既に完了しています。");
        IsCompleted = true;
        MarkUpdated();
    }

    public void Reopen()
    {
        if (!IsCompleted)
            throw new InvalidOperationException("このタスクはまだ完了していません。");
        IsCompleted = false;
        MarkUpdated();
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("タイトルは必須です。", nameof(title));
        if (title.Trim().Length > 100)
            throw new ArgumentException("タイトルは100文字以内で入力してください。", nameof(title));
    }
}
