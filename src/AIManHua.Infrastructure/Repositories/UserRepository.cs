using AIManHua.Domain.Entities;
using AIManHua.Domain.Interfaces;
using AIManHua.Infrastructure.Data;
using AIManHua.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace AIManHua.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    private readonly SnowflakeIdGenerator _idGenerator;

    public UserRepository(AppDbContext db, SnowflakeIdGenerator idGenerator)
    {
        _db = db;
        _idGenerator = idGenerator;
    }

    public async Task<User?> GetByIdAsync(long id) =>
        await _db.Users.FindAsync(id);

    public async Task<User?> GetByEmailAsync(string email) =>
        await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<bool> EmailExistsAsync(string email) =>
        await _db.Users.AnyAsync(u => u.Email == email);

    public async Task<bool> UsernameExistsAsync(string username) =>
        await _db.Users.AnyAsync(u => u.Username == username);

    public async Task<User> AddAsync(User user)
    {
        user.Id = _idGenerator.NextId();
        user.CreatedAt = DateTime.UtcNow;
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
    }
}
