namespace Chat.Web.Models;

public class UserViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    
    public IEnumerable<string> JoinedRooms { get; set; } = null!;
    public IEnumerable<string> CreatedRooms { get; set; } = null!;
}