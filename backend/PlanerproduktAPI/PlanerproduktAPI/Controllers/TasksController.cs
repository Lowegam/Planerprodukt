using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanerproduktAPI.Data;
using PlanerproduktAPI.Models;
using System.Security.Claims;

namespace PlanerproduktAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public TasksController(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

    [HttpGet]
    public async Task<IActionResult> GetTasks(
        [FromQuery] string? status,
        [FromQuery] string? priority,
        [FromQuery] string? search)
    {
        var userId = GetUserId();
        var query = _context.Tasks
            .Include(t => t.Category)
            .Where(t => t.UserId == userId);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(t => t.Status.ToString() == status);
        if (!string.IsNullOrEmpty(priority))
            query = query.Where(t => t.Priority.ToString() == priority);
        if (!string.IsNullOrEmpty(search))
            query = query.Where(t => t.Title.Contains(search) || (t.Description != null && t.Description.Contains(search)));

        var tasks = await query.OrderBy(t => t.Deadline).Select(t => new
        {
            t.Id,
            t.Title,
            t.Description,
            Status = t.Status.ToString(),
            Priority = t.Priority.ToString(),
            t.Deadline,
            t.CategoryId,
            CategoryName = t.Category != null ? t.Category.Name : null,
            t.CreatedAt
        }).ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var userId = GetUserId();
        var task = await _context.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task == null) return NotFound();
        return Ok(new
        {
            task.Id,
            task.Title,
            task.Description,
            Status = task.Status.ToString(),
            Priority = task.Priority.ToString(),
            task.Deadline,
            task.CategoryId,
            CategoryName = task.Category?.Name,
            task.CreatedAt
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = Enum.TryParse<TaskStatusEnum>(dto.Status, out var s) ? s : TaskStatusEnum.New,
            Priority = Enum.TryParse<PriorityEnum>(dto.Priority, out var p) ? p : PriorityEnum.Medium,
            Deadline = dto.Deadline,
            CategoryId = dto.CategoryId,
            UserId = GetUserId()
        };
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] CreateTaskDto dto)
    {
        var userId = GetUserId();
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (task == null) return NotFound();

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = Enum.TryParse<TaskStatusEnum>(dto.Status, out var s) ? s : task.Status;
        task.Priority = Enum.TryParse<PriorityEnum>(dto.Priority, out var p) ? p : task.Priority;
        task.Deadline = dto.Deadline;
        task.CategoryId = dto.CategoryId;
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Zadanie zaktualizowane" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userId = GetUserId();
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (task == null) return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Zadanie usuniete" });
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var userId = GetUserId();
        var tasks = await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();

        return Ok(new
        {
            Total = tasks.Count,
            ByStatus = new
            {
                New = tasks.Count(t => t.Status == TaskStatusEnum.New),
                InProgress = tasks.Count(t => t.Status == TaskStatusEnum.InProgress),
                Done = tasks.Count(t => t.Status == TaskStatusEnum.Done)
            },
            ByPriority = new
            {
                Low = tasks.Count(t => t.Priority == PriorityEnum.Low),
                Medium = tasks.Count(t => t.Priority == PriorityEnum.Medium),
                High = tasks.Count(t => t.Priority == PriorityEnum.High)
            },
            CompletionRate = tasks.Count > 0 ? Math.Round((double)tasks.Count(t => t.Status == TaskStatusEnum.Done) / tasks.Count * 100, 1) : 0
        });
    }
}

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public DateTime? Deadline { get; set; }
    public int? CategoryId { get; set; }
}