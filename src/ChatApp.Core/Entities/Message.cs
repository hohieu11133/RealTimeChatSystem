namespace ChatApp.Core.Entities;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    // FK
    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;

    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;
}
