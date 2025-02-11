using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListApi.Models;

namespace TodoApi.Services
{
    public interface ITodoService
    {
        Task<User> Login(string username, string password);

        Task<List<TodoItem>> GetTodos();

        Task<TodoItem> GetTodoById(int id);

        Task CreateTodo(string title, bool isCompleted);

        Task UpdateTodo(int todoId, TodoItem todo);

        Task DeleteTodo(int todoId);
    }
}
