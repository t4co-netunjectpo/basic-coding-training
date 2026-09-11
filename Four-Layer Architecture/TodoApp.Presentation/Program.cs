using TodoApp.Application;
using TodoApp.Application.Todos;
using TodoApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication().AddInfrastructure();
var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
var group = app.MapGroup("/api/todos").WithTags("Todos");

group.MapGet("/", async (ITodoService service, CancellationToken ct) => Results.Ok(await service.GetAllAsync(ct)));
group.MapGet("/{id:guid}", async (Guid id, ITodoService service, CancellationToken ct) =>
    (await service.GetAsync(id, ct)) is { } todo ? Results.Ok(todo) : Results.NotFound());
group.MapPost("/", async (CreateTodoRequest request, ITodoService service, CancellationToken ct) =>
{
    var errors = TodoRequestValidator.Validate(request.Title);
    if (errors.Count > 0) return Results.ValidationProblem(errors);
    var created = await service.CreateAsync(request, ct);
    return Results.Created($"/api/todos/{created.Id}", created);
});
group.MapPut("/{id:guid}", async (Guid id, UpdateTodoRequest request, ITodoService service, CancellationToken ct) =>
{
    var errors = TodoRequestValidator.Validate(request.Title);
    if (errors.Count > 0) return Results.ValidationProblem(errors);
    await service.UpdateAsync(id, request, ct);
    return Results.NoContent();
});
group.MapPost("/{id:guid}/complete", async (Guid id, ITodoService service, CancellationToken ct) =>
{
    await service.CompleteAsync(id, ct);
    return Results.NoContent();
});
group.MapDelete("/{id:guid}", async (Guid id, ITodoService service, CancellationToken ct) =>
{
    await service.DeleteAsync(id, ct);
    return Results.NoContent();
});

app.Run();

public partial class Program;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception) when (exception is ArgumentException or KeyNotFoundException or InvalidOperationException)
        {
            context.Response.StatusCode = exception switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                InvalidOperationException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status400BadRequest
            };
            await context.Response.WriteAsJsonAsync(new
            {
                error = exception.Message,
                status = context.Response.StatusCode
            });
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unexpected exception while processing {Path}", context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "予期しないエラーが発生しました。",
                status = StatusCodes.Status500InternalServerError
            });
        }
    }
}
