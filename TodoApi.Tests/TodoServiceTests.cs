using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TodoListApi.Services;
using TodoListApi.Models;
using TodoApi.Services;

public class TodoServiceTests
{
    private readonly Mock<ITodoService> _todoServiceMock;

    public TodoServiceTests()
    {
        _todoServiceMock = new Mock<ITodoService>();
    }

    [Fact]
    public async Task GetTodos_ReturnsListOfTodos()
    {
        var mockTodos = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Test Todo", IsCompleted = false }
        };

        _todoServiceMock.Setup(service => service.GetTodos()).ReturnsAsync(mockTodos);
        var result = await _todoServiceMock.Object.GetTodos();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Todo", result[0].Title);
    }

    [Fact]
    public async Task GetTodoById_ReturnsCorrectTodo()
    {
        var mockTodo = new TodoItem { Id = 1, Title = "Test Todo", IsCompleted = false };

        _todoServiceMock.Setup(service => service.GetTodoById(1)).ReturnsAsync(mockTodo);
        var result = await _todoServiceMock.Object.GetTodoById(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task CreateTodo_AddsTodoSuccessfully()
    {
        var newTodo = new TodoItem { Title = "New Todo", IsCompleted = false };

        _todoServiceMock.Setup(service => service.CreateTodo(newTodo.Title, newTodo.IsCompleted))
                        .Returns(Task.CompletedTask);

        await _todoServiceMock.Object.CreateTodo(newTodo.Title, newTodo.IsCompleted);

        _todoServiceMock.Verify(service => service.CreateTodo(newTodo.Title, newTodo.IsCompleted), Times.Once);
    }

    [Fact]
    public async Task UpdateTodo_UpdatesTodoSuccessfully()
    {
        var todoToUpdate = new TodoItem { Id = 1, Title = "Updated Todo", IsCompleted = true };

        _todoServiceMock.Setup(service => service.UpdateTodo(1, todoToUpdate))
                        .Returns(Task.CompletedTask);

        await _todoServiceMock.Object.UpdateTodo(1, todoToUpdate);

        _todoServiceMock.Verify(service => service.UpdateTodo(1, todoToUpdate), Times.Once);
    }

    [Fact]
    public async Task DeleteTodo_DeletesTodoSuccessfully()
    {
        _todoServiceMock.Setup(service => service.DeleteTodo(1)).Returns(Task.CompletedTask);

        await _todoServiceMock.Object.DeleteTodo(1);

        _todoServiceMock.Verify(service => service.DeleteTodo(1), Times.Once);
    }
}
