using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _db;

    public TaskRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await  _db.Tasks.ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(TaskItem task)
    {
        await _db.Tasks.AddAsync(task);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItem task)
    {
        _db.Tasks.Update(task);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
       var task = await _db.Tasks.FindAsync(id);
       if (task != null)
       {
            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
       }
       
    }
}