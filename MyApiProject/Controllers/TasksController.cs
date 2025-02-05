using Microsoft.AspNetCore.Mvc;
using MyApiProject.Models;
using MyApiProject.Services.Interfaces;


namespace MyApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/tasks/project/{projectId}/status/{status}
        [HttpGet("project/{projectId}/status/{status}")]
        public async Task<ActionResult<IEnumerable<ProjectTask>>> GetTasksByProjectIdAndStatus(int projectId,  string status)
        {
            try
            {
                var tasks = await _taskService.GetTasksByProjectIdAndStatusAsync(projectId, status);
                return Ok(tasks);
            }
            catch (KeyNotFoundException ex)  
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/tasks
        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<ProjectTask>>> GetTasksByProjectId(int projectId)
        {
            try
            {
                var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);
                return Ok(tasks);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectTask>> GetTaskById(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                return Ok(task);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        // POST: api/tasks/project/{projectId}
        [HttpPost("project/{projectId}")]
        public async Task<ActionResult<Task>> CreateTask(int projectId, [FromBody] ProjectTask task)
        {
            try
            {
                var createdTask = await _taskService.CreateTaskAsync(task, projectId);
                return Ok(createdTask);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch(InvalidOperationException ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
        }

        // PUT: api/tasks/{id}/project/{projectId}
        [HttpPut("{taskId}/project/{projectId}")]
        public async Task<IActionResult> UpdateTask(int taskId, ProjectTask updatedTask, int projectId)
        {
            try
            {
                var updtdTask = await _taskService.UpdateTaskAsync(taskId, updatedTask, projectId);
                return Ok(updtdTask);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{taskId}/project/{projectId}")]
        public async Task<IActionResult> DeleteTask(int taskId, int projectId)
        {
            try
            {
                var result = await _taskService.DeleteTaskAsync(taskId, projectId);

                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
