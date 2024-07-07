namespace Chat.BLL.Models;

public class UserModel : BaseModel
{
    public string FullName { get; set; } = null!;
    
    public IEnumerable<RoomModel> JoinedRooms { get; set; } = null!;
    public IEnumerable<RoomModel> CreatedRooms { get; set; } = null!;
    public IEnumerable<MessageModel> Messages { get; set; } = null!;
}