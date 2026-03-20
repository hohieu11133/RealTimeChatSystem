namespace ChatApp.Shared.DTOs;

public class MessageDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string SenderUsername { get; set; } = string.Empty;
    public int SenderId { get; set; }
    public int RoomId { get; set; }
    public DateTime SentAt { get; set; }
}
