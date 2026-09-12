using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TodoApp.Application.Todos;

namespace TodoApp_WinUI.ViewModels;

/// <summary>
/// Sample ViewModel using CommunityToolkit.Mvvm partial property syntax.
/// Uses <see cref="ObservableProperty"/> for change notification and
/// <see cref="RelayCommand"/> for command binding.
/// </summary>
public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string NewTitle { get; set; } = string.Empty;

    [RelayCommand]
    private async Task AddAsync()
    {
        try
        {
            var todo = await todoService.CreateAsync(new CreateTodoRequest(NewTitle, NewDescription));
            Todos.Add(todo);
            NewTitle = string.Empty;
            NewDescription = string.Empty;
            ErrorMessage = string.Empty;
        }
        catch (ArgumentException exception)
        {
            ErrorMessage = exception.Message;
        }
        catch (InvalidOperationException exception)
        {
            ErrorMessage = exception.Message;
        }
    }

    [RelayCommand]
    private async Task CompleteAsync(TodoDto todo)
    {
        try
        {
            await todoService.CompleteAsync(todo.Id);
            await RefreshAsync();
        }
        catch (InvalidOperationException exception)
        {
            ErrorMessage = exception.Message;
        }
        catch (KeyNotFoundException exception)
        {
            ErrorMessage = exception.Message;
        }
    }

    [RelayCommand]
    private async Task DeleteAsync(TodoDto todo)
    {
        try
        {
            await todoService.DeleteAsync(todo.Id);
            Todos.Remove(todo);
            ErrorMessage = string.Empty;
        }
        catch (KeyNotFoundException exception)
        {
            ErrorMessage = exception.Message;
        }
    }

    public ObservableCollection<TodoDto> Todos { get; } = [];

    [ObservableProperty]
    public partial string NewDescription { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    private readonly ITodoService todoService = App.TodoService;

    public Task InitializeAsync() => RefreshAsync();

    private async Task RefreshAsync()
    {
        var todos = await todoService.GetAllAsync();
        Todos.Clear();
        foreach (var todo in todos)
            Todos.Add(todo);
        ErrorMessage = string.Empty;
    }

    partial void OnErrorMessageChanged(string value) => OnPropertyChanged(nameof(HasError));
}
