using Chat.BLL.Models;

namespace Chat.BLL.Services.Interfaces;

public interface IUserService : IBaseService<UserModel>
{
    Task<UserModel> CreateUserAsync(UserModel userModel, CancellationToken cancellationToken = default);
    Task<UserModel> UpdateUserAsync(int userId, UserModel userModel, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(int userId, CancellationToken cancellationToken = default);
}