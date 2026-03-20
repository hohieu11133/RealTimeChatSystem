using ChatApp.Core.Entities;

namespace ChatApp.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User> AddAsync(User user);
    Task<bool> ExistsAsync(string username);
}
