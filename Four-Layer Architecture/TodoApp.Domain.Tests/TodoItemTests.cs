using TodoApp.Domain.Todos;

namespace TodoApp.Domain.Tests;

public class TodoItemTests
{
    [Fact]
    public void Constructor_rejects_blank_title()
    {
        Assert.Throws<ArgumentException>(() => new TodoItem(" "));
    }

    [Fact]
    public void Complete_twice_is_a_conflict()
    {
        var todo = new TodoItem("学習");
        todo.Complete();
        Assert.Throws<InvalidOperationException>(todo.Complete);
    }
}
