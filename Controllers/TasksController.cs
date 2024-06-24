using Kolokwium.Exceptions;
using Kolokwium.Services;
using Microsoft.AspNetCore.Mvc;
using Task = Kolokwium.Models.Task;

namespace Kolokwium.Controllers
{
    [ApiController]
    [Route("api/task")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTasks([FromQuery] int? projectId)
        {
            var tasks = _taskService.GetTasks(projectId);
            return Ok(tasks);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AddTask([FromBody] TaskDto taskDto)
        {
            var task = new Task
            {
                Name = taskDto.Name,
                Description = taskDto.Description,
                CreatedAt = DateTime.Now,
                IdProject = taskDto.IdProject,
                IdReporter = taskDto.IdReporter,
                IdAssignee = taskDto.IdAssignee
            };

            try
            {
                _taskService.AddTask(task);
                return CreatedAtAction(nameof(GetTasks), new { id = task.IdTask }, task);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }

    public class TaskDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int IdProject { get; set; }
        public int IdReporter { get; set; }
        public int IdAssignee { get; set; }
    }
}