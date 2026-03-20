using System.Security.Claims;
using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces;
using ChatApp.Shared.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Hubs;

public class ChatHub : Hub
{
    private readonly IMessageRepository _messageRepo;
    private readonly IUserRepository _userRepo;

    public ChatHub(IMessageRepository messageRepo, IUserRepository userRepo)
    {
        _messageRepo = messageRepo;
        _userRepo = userRepo;
    }

    private int GetUserId()
    {
        var claim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(claim ?? throw new HubException("Unauthorized"));
    }

    private string GetUsername()
        => Context.User?.FindFirst(ClaimTypes.Name)?.Value
           ?? throw new HubException("Unauthorized");

    public async Task JoinRoom(int roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        await Clients.Group(roomId.ToString())
            .SendAsync("UserJoined", GetUsername());
    }

    public async Task LeaveRoom(int roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        await Clients.Group(roomId.ToString())
            .SendAsync("UserLeft", GetUsername());
    }

    public async Task SendMessage(int roomId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new HubException("Message cannot be empty.");

        var userId = GetUserId();

        var message = new Message
        {
            Content = content.Trim(),
            RoomId = roomId,
            SenderId = userId,
            SentAt = DateTime.UtcNow
        };

        var saved = await _messageRepo.AddAsync(message);

        var dto = new MessageDto
        {
            Id = saved.Id,
            Content = saved.Content,
            SentAt = saved.SentAt,
            SenderId = saved.SenderId,
            SenderUsername = saved.Sender.Username,
            RoomId = saved.RoomId
        };

        await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", dto);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
