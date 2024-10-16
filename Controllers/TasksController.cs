using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public TasksController(TaskDbContext context)
        {
            _context = context;
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskModel task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaskById), new { id = task.TaskId }, task);
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetAllTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return Ok(tasks);
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModel>> GetTaskById(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskModel updatedTask)
        {
            if (id != updatedTask.TaskId)
            {
                return BadRequest("Task ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            // Update the task properties
            task.title = updatedTask.title;
            task.description = updatedTask.description;
            task.AssignedTo = updatedTask.AssignedTo;
            task.priority = updatedTask.priority;
            task.duedate = updatedTask.duedate;
            task.status = updatedTask.status;

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TaskExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> TaskExists(int id)
        {
            return await _context.Tasks.AnyAsync(e => e.TaskId == id);
        }
    }
}
