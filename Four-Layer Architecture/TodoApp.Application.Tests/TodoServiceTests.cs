using TodoApp.Application.Todos;
using TodoApp.Infrastructure.Persistence;

namespace TodoApp.Application.Tests;

public class TodoServiceTests
{
    [Fact]
    public async Task Create_and_get_round_trip_through_repository()
    {
        var service = new TodoService(new InMemoryTodoRepository());
        var created = await service.CreateAsync(new CreateTodoRequest("買い物", "牛乳"));

        var loaded = await service.GetAsync(created.Id);

        Assert.NotNull(loaded);
        Assert.Equal("買い物", loaded.Title);
        Assert.Equal("牛乳", loaded.Description);
    }

    [Fact]
    public async Task Missing_todo_is_not_found()
    {
        var service = new TodoService(new InMemoryTodoRepository());
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(Guid.NewGuid()));
    }
}
