using ChatApp.Core.Entities;

namespace ChatApp.Core.Interfaces;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetByRoomAsync(int roomId, int page = 1, int pageSize = 50);
    Task<Message> AddAsync(Message message);
}
