using AutoMapper;
using Chat.BLL.Exceptions;
using Chat.BLL.Helpers;
using Chat.BLL.Models;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.BLL.Services;

public class RoomService(IRoomRepository roomRepository, IMapper mapper, IUserRepository userRepository) : IRoomService
{
    public async Task<RoomModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return mapper.Map<RoomModel>(await roomRepository.GetByIdAsync(id, cancellationToken));
    }

    public async Task<RoomModel> CreateRoomAsync(int userId, RoomModel roomModel, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with Id {userId} not found");

        roomModel.CreatorId = userDb.Id;
        var roomDb = await roomRepository.CreateAsync(mapper.Map<Room>(roomModel), cancellationToken);

        return mapper.Map<RoomModel>(roomDb);
    }
    
    public async Task<RoomModel> UpdateRoomAsync(int userId, int roomId, RoomModel roomModel, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with Id {userId} not found");
        
        var roomDb = await roomRepository.GetByIdAsync(roomId, cancellationToken);
        if (roomDb is null)
            throw new RoomNotFoundException($"Room with Id {roomId} not found");
        
        if (roomDb.CreatorId != userDb.Id)
            throw new NoPermissionsException("There are no permissions to do the operation");
        
        foreach (var propertyMap in ReflectionHelper.WidgetUtil<RoomModel, Room>.PropertyMap)
        {
            var roomProperty = propertyMap.Item1;
            var roomDbProperty = propertyMap.Item2;

            var roomSourceValue = roomProperty.GetValue(roomModel);
            var roomTargetValue = roomDbProperty.GetValue(roomDb);

            if (roomSourceValue != null && 
                !ReferenceEquals(roomSourceValue, "") &&
                !roomSourceValue.Equals(roomTargetValue))
            {
                roomDbProperty.SetValue(roomDb, roomSourceValue);
            }
        }

        var room = await roomRepository.UpdateAsync(roomDb, cancellationToken);
        return mapper.Map<RoomModel>(room);
    }
    
    public async Task DeleteRoomAsync(int userId, int roomId, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with Id {userId} not found");
        
        var roomDb = await roomRepository.GetByIdAsync(roomId, cancellationToken);
        if (roomDb is null)
            throw new RoomNotFoundException($"Room with Id {roomId} not found");

        if (roomDb.CreatorId != userDb.Id)
            throw new NoPermissionsException("There are no permissions to do the operation");

        await roomRepository.DeleteAsync(roomDb, cancellationToken);
    } 

    public async Task JoinRoomAsync(int userId, int roomId, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with Id {userId} not found");
        
        var roomDb = await roomRepository.GetByIdAsync(roomId, cancellationToken);
        if (roomDb is null)
            throw new RoomNotFoundException($"Room with Id {roomId} not found");
        
        roomDb.Users.Add(userDb);
        await roomRepository.UpdateAsync(roomDb, cancellationToken);
    }

    public async Task LeaveRoomAsync(int userId, int roomId, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with Id {userId} not found");
        
        var roomDb = await roomRepository.GetByIdAsync(roomId, cancellationToken);
        if (roomDb is null)
            throw new RoomNotFoundException($"Room with Id {roomId} not found");

        if (!roomDb.Users.Contains(userDb))
            throw new UserInRoomException($"User with Id {userId} is not a room member");
        
        roomDb.Users.Remove(userDb);
        await roomRepository.UpdateAsync(roomDb, cancellationToken);
    }

    public async Task<IEnumerable<RoomModel>> GetAllUsersRoomsAsync(int userId, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with Id {userId} not found");

        var rooms = await roomRepository.GetAll().Where(r => r.CreatorId == userId || r.Users.Contains(userDb))
            .ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<RoomModel>>(rooms);
    }
}