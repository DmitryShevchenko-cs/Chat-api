namespace Chat.Web.Models;

public class MessageViewModel
{
    public int RoomId { get; set; }
    public string Room { get; set; } = null!;

    public int UserId { get; set; }
    public string User { get; set; } = null!;

    public string Text { get; set; } = null!;
}