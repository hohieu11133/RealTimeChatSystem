using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces;
using ChatApp.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/rooms")]
[Authorize]
public class RoomsController : ControllerBase
{
    private readonly IRoomRepository _roomRepo;
    private readonly IMessageRepository _messageRepo;

    public RoomsController(IRoomRepository roomRepo, IMessageRepository messageRepo)
    {
        _roomRepo = roomRepo;
        _messageRepo = messageRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rooms = await _roomRepo.GetAllAsync();
        var dtos = rooms.Select(r => new RoomDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            CreatedAt = r.CreatedAt
        });
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
            return BadRequest("Room name is required.");

        var room = new Room
        {
            Name = req.Name.Trim(),
            Description = req.Description?.Trim() ?? string.Empty
        };

        var created = await _roomRepo.AddAsync(room);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, new RoomDto
        {
            Id = created.Id,
            Name = created.Name,
            Description = created.Description,
            CreatedAt = created.CreatedAt
        });
    }

    [HttpGet("{id}/messages")]
    public async Task<IActionResult> GetMessages(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var room = await _roomRepo.GetByIdAsync(id);
        if (room is null) return NotFound("Room not found.");

        var messages = await _messageRepo.GetByRoomAsync(id, page, pageSize);
        var dtos = messages.Select(m => new MessageDto
        {
            Id = m.Id,
            Content = m.Content,
            SentAt = m.SentAt,
            SenderId = m.SenderId,
            SenderUsername = m.Sender.Username,
            RoomId = m.RoomId
        });
        return Ok(dtos);
    }
}
