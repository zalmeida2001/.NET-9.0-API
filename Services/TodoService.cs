using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TodoListApi.Data;
using TodoListApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Services;

namespace TodoListApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly TodoContext _context;
        private readonly IDistributedCache _cache;

        public TodoService(TodoContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<User> Login(string username, string password)
        {
            return await _context.LoginAsync(username, password);
        }

        public async Task<List<TodoItem>> GetTodos()
        {
            string cacheKey = "AllTodos";
            string cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<List<TodoItem>>(cachedData);
            }

            var todos = await _context.GetTodosAsync();

            if (todos != null && todos.Count > 0)
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(todos), cacheOptions);
            }

            return todos;
        }

        public async Task<TodoItem> GetTodoById(int id)
        {
            string cacheKey = $"TodoItem_{id}";
            string cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<TodoItem>(cachedData);
            }

            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem != null)
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(todoItem), cacheOptions);
            }

            return todoItem;
        }

        public async Task CreateTodo(string title, bool isCompleted)
        {
            await _context.CreateTodoAsync(title, isCompleted);
            await _cache.RemoveAsync("AllTodos"); // Invalidate all todos cache
        }

        public async Task UpdateTodo(int todoId, TodoItem todo)
        {
            await _context.UpdateTodoAsync(todoId, todo.Title, todo.IsCompleted);
            await _cache.RemoveAsync("AllTodos"); // Invalidate all todos cache
            await _cache.RemoveAsync($"TodoItem_{todoId}"); // Invalidate specific todo cache
        }

        public async Task DeleteTodo(int todoId)
        {
            await _context.DeleteTodoAsync(todoId);
            await _cache.RemoveAsync("AllTodos"); // Invalidate all todos cache
            await _cache.RemoveAsync($"TodoItem_{todoId}"); // Invalidate specific todo cache
        }
    }
}
