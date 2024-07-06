namespace Chat.DAL.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = null!;
    
    public IEnumerable<Room> JoinedRooms { get; set; } = null!;
    public IEnumerable<Room> CreatedRooms { get; set; } = null!;
    public IEnumerable<Message> Messages { get; set; } = null!;
    
}