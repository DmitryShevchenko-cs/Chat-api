using AutoMapper;
using Chat.BLL.Exceptions;
using Chat.BLL.Helper;
using Chat.BLL.Models;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;

namespace Chat.BLL.Services;

public class UserService(IUserRepository userRepository,  IMapper mapper) : IUserService
{
    public async Task<UserModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return mapper.Map<UserModel>(await userRepository.GetByIdAsync(id, cancellationToken));
    }

    public async Task<UserModel> CreateUserAsync(UserModel userModel, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.CreateAsync(mapper.Map<User>(userModel), cancellationToken);

        return mapper.Map<UserModel>(userDb);
    }
    
    public async Task<UserModel> UpdateUserAsync(int userId, UserModel userModel, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with Id {userId} not found");
        
        foreach (var propertyMap in ReflectionHelper.WidgetUtil<UserModel, User>.PropertyMap)
        {
            var roomProperty = propertyMap.Item1;
            var roomDbProperty = propertyMap.Item2;

            var roomSourceValue = roomProperty.GetValue(userModel);
            var roomTargetValue = roomDbProperty.GetValue(userDb);

            if (roomSourceValue != null && 
                !ReferenceEquals(roomSourceValue, "") &&
                !roomSourceValue.Equals(roomTargetValue))
            {
                roomDbProperty.SetValue(userDb, roomSourceValue);
            }
        }
        
        var user = await userRepository.UpdateAsync(userDb, cancellationToken);
        return mapper.Map<UserModel>(user);
    }

    public async Task DeleteUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with Id {userId} not found");

        await userRepository.DeleteAsync(userDb, cancellationToken);
    }
}