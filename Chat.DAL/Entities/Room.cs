namespace Chat.DAL.Entities;

public class Room : BaseEntity
{
    public string Name { get; set; } = null!;
    
    public int CreatorId { get; set; }
    public User Creator { get; set; } = null!;
    
    public ICollection<User> Users { get; set; } = null!;

    public IEnumerable<Message> Messages { get; set; } = null!;
}