using AutoMapper;
using Chat.BLL.Exceptions;
using Chat.BLL.Models;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.BLL.Services;

public class MessageService(IMessageRepository messageRepository, IUserRepository userRepository, IRoomRepository roomRepository, IMapper mapper)
    : IMessageService
{
    public async Task<MessageModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return mapper.Map<MessageModel>(await messageRepository.GetByIdAsync(id, cancellationToken));
    }

    public async Task<MessageModel> CreateMessageAsync(int userId, int roomId, string text, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with Id {userId} not found");

        var roomDb = await roomRepository.GetByIdAsync(roomId, cancellationToken);
        if (roomDb is null)
            throw new RoomNotFoundException($"Room with Id {roomId} not found");

        var messageDb = await messageRepository.CreateAsync(new Message
        {
            RoomId = roomId,
            UserId = userId,
            Text = text
        }, cancellationToken);
        
        return mapper.Map<MessageModel>(messageDb);
    }

    public async Task<IEnumerable<MessageModel>> GetChatMessagesAsync(int userId, int roomId, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with Id {userId} not found");

        var roomDb = await roomRepository.GetByIdAsync(roomId, cancellationToken);
        if (roomDb is null)
            throw new RoomNotFoundException($"Room with Id {roomId} not found");

        if (!roomDb.Users.Contains(userDb)) throw new UserOutOfRoomException("User is not in the room");
        
        var messages = await messageRepository.GetAll().Where(r => r.RoomId == roomId)
            .ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<MessageModel>>(messages);
    }
}