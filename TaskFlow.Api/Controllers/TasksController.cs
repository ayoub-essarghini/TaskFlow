using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

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
  public async Task<ActionResult<IEnumerable<TaskItem>>> GetTask(string id)
  {
    var task = await _taskRepository.GetByIdAsync(Guid.Parse(id));
    return Ok(task);
  }
}