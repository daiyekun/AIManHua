using AIManHua.Domain.Entities;

namespace AIManHua.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(long id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UsernameExistsAsync(string username);
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
}
