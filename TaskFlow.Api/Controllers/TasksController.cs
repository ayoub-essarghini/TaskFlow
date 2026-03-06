using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;
using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
  private readonly ITaskRepository _taskRepository;

  public TasksController(ITaskRepository taskRepository)
  {
    _taskRepository = taskRepository;
  }


  [HttpGet]
  public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
  {
    var tasks = await _taskRepository.GetAllAsync();
    return Ok(tasks);
  }

  [HttpPost]
  public async Task<ActionResult> CreateTask([FromBody] CreateTaskDto dto)
  {
    var task = new TaskItem
    {
      Title = dto.Title,
      Description = dto.Description,
    };
    await _taskRepository.AddAsync(task);
    return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<TaskResponseDto>> GetTaskById(Guid id)
  {
    var task = await _taskRepository.GetByIdAsync(id);
    
    if (task == null)
    {
      return NotFound(new { message = "Task not found." }); 
    }

    var response = new TaskResponseDto
    {
      Id = task.Id,
      Title = task.Title,
      Description = task.Description,
      Status = task.Status.ToString(),
    };

 
    return Ok(response);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult> UpdateTaskById([FromBody] CreateTaskDto dto, Guid id)
  {
    var task = _taskRepository.GetByIdAsync(id);
    if (task == null)
    {
      return NotFound(new {message = "Not found."});
    }

    if (!Enum.TryParse<TaskStatus>(dto.Status, true, out var status)) return BadRequest();
    var newTask = new TaskItem
    {
      Title = dto.Title,
      Description = dto.Description,
      Status = status
    };
    await _taskRepository.UpdateAsync(newTask);
    return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteTaskById(Guid id)
  {
    var task = _taskRepository.GetByIdAsync(id);
    if (task == null)
    {
      return NotFound();
    }
    await _taskRepository.DeleteAsync(id);
    return NoContent();
  }
}