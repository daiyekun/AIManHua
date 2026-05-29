using AIManHua.Domain.Entities;

namespace AIManHua.Domain.Interfaces;

public interface IComicTaskRepository
{
    Task<ComicTask?> GetByIdAsync(long id);
    Task<IEnumerable<ComicTask>> GetByUserIdAsync(long userId);
    Task<ComicTask> AddAsync(ComicTask task);
    Task UpdateAsync(ComicTask task);
}
