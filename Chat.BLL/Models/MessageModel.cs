namespace Chat.BLL.Models;

public class MessageModel : BaseModel
{
    public int RoomId { get; set; }
    public RoomModel Room { get; set; } = null!;

    public int UserId { get; set; }
    public UserModel User { get; set; } = null!;

    public string Text { get; set; } = null!;
}