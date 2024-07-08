namespace Chat.Web.Models;

public class RoomViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    public int CreatorId { get; set; }
    public UserViewModel Creator { get; set; } = null!;
    
    public IEnumerable<UserViewModel> Users { get; set; } = null!;
}