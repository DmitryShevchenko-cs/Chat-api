using Chat.BLL.Models;

namespace Chat.BLL.Services.Interfaces;

public interface IMessageService :  IBaseService<MessageModel>
{
    Task<MessageModel> CreateMessageAsync(int userId, int roomId, string text, CancellationToken cancellationToken);
    
    Task<IEnumerable<MessageModel>> GetChatMessagesAsync(int userId, int roomId, CancellationToken cancellationToken);
}