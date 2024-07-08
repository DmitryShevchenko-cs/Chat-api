using Chat.BLL.Models;

namespace Chat.BLL.Services.Interfaces;

public interface IMessageService : IBaseService<MessageModel>
{
    Task<MessageModel> CreateMessageAsync(int userId, int roomId, string text, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<MessageModel>> GetRoomMessagesAsync(int userId, int roomId, CancellationToken cancellationToken = default);
}