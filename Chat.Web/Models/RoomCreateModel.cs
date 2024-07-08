namespace Chat.Web.Models;

public class RoomCreateModel
{
    public int CreatorId { get; set; }
    public string Name { get; set; } = null!;
}