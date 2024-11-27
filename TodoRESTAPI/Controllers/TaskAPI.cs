using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoRestApi.Models;

namespace TodoRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private static List<Task> tasks = new List<Task>();
        private static int idCounter = 1;


        [HttpPost]
        public IActionResult CreateTask([FromBody] Task newTask)
        {
            if (string.IsNullOrEmpty(newTask.Title))
            {
                return BadRequest(new { error = "Title is required" });
            }

            newTask.Id = idCounter++;
            newTask.Status = false;
            tasks.Add(newTask);

            return CreatedAtAction(nameof(GetTaskById), new { id = newTask.Id }, newTask);
        }


        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok(tasks);
        }


        [HttpGet("{id}")]
        public IActionResult GetTaskById(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound(new { error = "Task not found" });
            }

            return Ok(task);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody] Task updatedTask)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound(new { error = "Task not found" });
            }

            if (!string.IsNullOrEmpty(updatedTask.Title))
            {
                task.Title = updatedTask.Title;
            }

            task.Status = updatedTask.Status;
            return Ok(task);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound(new { error = "Task not found" });
            }

            tasks.Remove(task);
            return NoContent();
        }
    }
}
