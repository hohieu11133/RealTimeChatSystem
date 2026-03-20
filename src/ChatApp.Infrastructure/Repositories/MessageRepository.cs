using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _db;
    public MessageRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Message>> GetByRoomAsync(int roomId, int page = 1, int pageSize = 50)
        => await _db.Messages
            .Where(m => m.RoomId == roomId)
            .Include(m => m.Sender)
            .OrderByDescending(m => m.SentAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(m => m.SentAt)
            .ToListAsync();

    public async Task<Message> AddAsync(Message message)
    {
        _db.Messages.Add(message);
        await _db.SaveChangesAsync();
        await _db.Entry(message).Reference(m => m.Sender).LoadAsync();
        return message;
    }
}
