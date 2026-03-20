using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public async Task<User?> GetByIdAsync(int id)
        => await _db.Users.FindAsync(id);

    public async Task<User?> GetByUsernameAsync(string username)
        => await _db.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User> AddAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<bool> ExistsAsync(string username)
        => await _db.Users.AnyAsync(u => u.Username == username);
}
