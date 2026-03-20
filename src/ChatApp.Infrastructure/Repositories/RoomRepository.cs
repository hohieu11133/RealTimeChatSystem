using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _db;
    public RoomRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Room>> GetAllAsync()
        => await _db.Rooms.OrderBy(r => r.Name).ToListAsync();

    public async Task<Room?> GetByIdAsync(int id)
        => await _db.Rooms.FindAsync(id);

    public async Task<Room> AddAsync(Room room)
    {
        _db.Rooms.Add(room);
        await _db.SaveChangesAsync();
        return room;
    }
}
