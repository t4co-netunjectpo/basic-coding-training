using Microsoft.Extensions.DependencyInjection;
using TodoApp.Domain.Todos;
using TodoApp.Infrastructure.Persistence;

namespace TodoApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
        services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
}
