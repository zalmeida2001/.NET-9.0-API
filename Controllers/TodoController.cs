using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Services;
using TodoListApi.Models;
using TodoListApi.Services;

namespace TodoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TodoItem>>> GetTodos()
        {
            var todos = await _todoService.GetTodos();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoById(int id)
        {
            var todo = await _todoService.GetTodoById(id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateTodo([FromBody] TodoItem todo)
        {
            await _todoService.CreateTodo(todo.Title, todo.IsCompleted);
            return CreatedAtAction(nameof(GetTodos), new { id = todo.Id }, todo);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTodo(int id, [FromBody] TodoItem todo)
        {
            await _todoService.UpdateTodo(id, todo);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            await _todoService.DeleteTodo(id);
            return NoContent();
        }
    }
}
