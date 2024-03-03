using todo_library.Data;
using todo_library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace todo_api.Controllers
{
    [Route("api/todos")]
    [ApiController]
    [Authorize]

    public class ToDosController(DataContext context) : ControllerBase
    {
        [HttpPost("addToDo")]
        public async Task<IActionResult> AddToDo(string title)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(title)) { return BadRequest("Title is empty!"); }
            if (!await context.Users.AnyAsync(c => c.Id == userId)) { return NotFound("User not found!"); }

            var todo = new ToDo();

            todo.UserId = userId;
            todo.Title = title;

            await context.ToDos.AddAsync(todo);
            await context.SaveChangesAsync();

            return Ok(todo);
        }

        [HttpGet("getToDo/{id}")]
        public async Task<IActionResult> GetToDo(string id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var todo = await context.ToDos.FindAsync(id);

            if (todo == null) { return NotFound("ToDo not found!"); }
            if (todo.UserId != userId) { return BadRequest("Wrong ToDo!"); }

            return Ok(todo);
        }

        [HttpGet("getAllToDos")]
        public async Task<IActionResult> GetAllToDos()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await context.Users.AnyAsync(c => c.Id == id)) { return NotFound("User not found!"); }

            var todos = await context.ToDos.Where(todo => todo.UserId == id)
                .ToListAsync();

            if (!todos.Any()) { return NotFound("ToDos not found!"); }

            return Ok(todos);
        }

        [HttpGet("getAllFinishedToDos")]
        public async Task<IActionResult> GetAllFinishedToDos()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await context.Users.AnyAsync(c => c.Id == id)) { return NotFound("User not found!"); }

            var todos = await context.ToDos.Where(todo => todo.UserId == id && todo.IsDone == true).ToListAsync();

            if (!todos.Any()) { return NotFound("ToDos not found!"); }

            return Ok(todos);
        }

        [HttpGet("getAllUnfinishedToDos")]
        public async Task<IActionResult> GetAllUnfinishedToDos()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await context.Users.AnyAsync(c => c.Id == id)) { return NotFound("User not found!"); }

            var todos = await context.ToDos.Where(todo => todo.UserId == id && todo.IsDone == false).ToListAsync();

            if (!todos.Any()) { return NotFound("ToDos not found!"); }

            return Ok(todos);
        }

        [HttpGet("changeIsDone/{id}")]
        public async Task<IActionResult> ChangeIsDone(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var todo = await context.ToDos.FindAsync(id);

            if (todo == null) { return NotFound("ToDo not found!"); }
            if (todo.UserId != userId) { return BadRequest("Wrong ToDo!"); }

            if (!todo.IsDone) { todo.IsDone = true; }
            else { todo.IsDone = false; }

            await context.SaveChangesAsync();

            return Ok(todo);
        }

        [HttpPut("changeTitle/{id}")]
        public async Task<IActionResult> ChangeTitle(int id, string title)
        {
            string userId = (User.FindFirstValue(ClaimTypes.NameIdentifier));
            var todo = await context.ToDos.FindAsync(id);

            if (todo == null) { return NotFound("ToDo not found!"); }
            if (todo.UserId != userId) { return BadRequest("Wrong ToDo!"); }
            if (todo.Title == title) { return BadRequest("Title didn't change!"); }
            if (string.IsNullOrWhiteSpace(title)) { return BadRequest("Title is empty!"); }

            todo.Title = title;

            await context.SaveChangesAsync();

            return Ok(todo);
        }

        [HttpDelete("deleteToDo/{id}")]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var todo = await context.ToDos.FindAsync(id);

            if (todo == null) { return NotFound("ToDo not found!"); }
            if (todo.UserId != userId) { return BadRequest("Wrong ToDo!"); }

            context.ToDos.Remove(todo);
            await context.SaveChangesAsync();

            return Ok(todo);
        }

        [HttpDelete("deleteAllToDos")]
        public async Task<IActionResult> DeleteAllToDos()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await context.Users.AnyAsync(c => c.Id == id)) { return NotFound("User not found!"); }

            var todos = await context.ToDos.Where(todo => todo.UserId == id)
                .ToListAsync();

            if (!todos.Any()) { return NotFound("ToDos not found!"); }

            foreach (var todo in todos) { context.ToDos.Remove(todo); }
            await context.SaveChangesAsync();

            return Ok(todos);
        }
    }
}
