namespace Chat.BLL.Models;

public class RoomModel : BaseModel
{
    public string Name { get; set; } = null!;
    
    public int CreatorId { get; set; }
    public UserModel Creator { get; set; } = null!;
    
    public IEnumerable<UserModel> Users { get; set; } = null!;
}