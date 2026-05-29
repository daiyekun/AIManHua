using AIManHua.Domain.Entities;
using AIManHua.Domain.Interfaces;
using AIManHua.Infrastructure.Data;
using AIManHua.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace AIManHua.Infrastructure.Repositories;

public class ComicTaskRepository : IComicTaskRepository
{
    private readonly AppDbContext _db;
    private readonly SnowflakeIdGenerator _idGenerator;

    public ComicTaskRepository(AppDbContext db, SnowflakeIdGenerator idGenerator)
    {
        _db = db;
        _idGenerator = idGenerator;
    }

    public async Task<ComicTask?> GetByIdAsync(long id) =>
        await _db.ComicTasks.Include(t => t.Storyboards).Include(t => t.Images)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<IEnumerable<ComicTask>> GetByUserIdAsync(long userId) =>
        await _db.ComicTasks.Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt).ToListAsync();

    public async Task<ComicTask> AddAsync(ComicTask task)
    {
        task.Id = _idGenerator.NextId();
        task.CreatedAt = DateTime.UtcNow;
        _db.ComicTasks.Add(task);
        await _db.SaveChangesAsync();
        return task;
    }

    public async Task UpdateAsync(ComicTask task)
    {
        _db.ComicTasks.Update(task);
        await _db.SaveChangesAsync();
    }
}
