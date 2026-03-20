using ChatApp.Core.Entities;

namespace ChatApp.Core.Interfaces;

public interface IRoomRepository
{
    Task<IEnumerable<Room>> GetAllAsync();
    Task<Room?> GetByIdAsync(int id);
    Task<Room> AddAsync(Room room);
}
