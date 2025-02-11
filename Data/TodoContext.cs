using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TodoListApi.Models;

namespace TodoListApi.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<User> Users { get; set; }

        public async Task<List<TodoItem>> GetTodosAsync()
        {
            return await this.TodoItems.FromSqlRaw("EXEC sp_GetTodos").ToListAsync();
        }

        public async Task<TodoItem> GetTodoByIdAsync(int id)
        {
            var idParam = new SqlParameter("@Id", id);
            return await this.TodoItems.FromSqlRaw("EXEC sp_GetTodoById @Id", idParam).FirstOrDefaultAsync();
        }


        public async Task CreateTodoAsync(string title, bool isCompleted)
        {
            var titleParam = new SqlParameter("@Title", title);
            var isCompletedParam = new SqlParameter("@IsCompleted", isCompleted);

            await this.Database.ExecuteSqlRawAsync("EXEC sp_CreateTodo @Title, @IsCompleted", titleParam, isCompletedParam);
        }

        public async Task UpdateTodoAsync(int todoId, string title, bool isCompleted)
        {
            var todoIdParam = new SqlParameter("@Id", todoId);
            var titleParam = new SqlParameter("@Title", title);
            var isCompletedParam = new SqlParameter("@IsCompleted", isCompleted);

            await this.Database.ExecuteSqlRawAsync("EXEC sp_UpdateTodo @Id, @Title, @IsCompleted", todoIdParam, titleParam, isCompletedParam);
        }

        public async Task DeleteTodoAsync(int todoId)
        {
            var todoIdParam = new SqlParameter("@Id", todoId);
            await this.Database.ExecuteSqlRawAsync("EXEC sp_DeleteTodo @Id", todoIdParam);
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var usernameParam = new SqlParameter("@Username", username);
            var passwordParam = new SqlParameter("@Password", password);

            var user = await this.Users.FromSqlRaw("EXEC sp_Login @Username, @Password", usernameParam, passwordParam).FirstOrDefaultAsync();
            return user;
        }
    }
}
