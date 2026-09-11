using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Todos;

namespace TodoApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services.AddScoped<ITodoService, TodoService>();
}
