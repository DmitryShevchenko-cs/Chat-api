using Chat.BLL.Models;

namespace Chat.BLL.Services.Interfaces;

public interface IRoomService : IBaseService<RoomModel>
{
    Task<RoomModel> CreateRoomAsync(int userId, RoomModel roomModel, CancellationToken cancellationToken = default);
    Task<RoomModel> UpdateRoomAsync(int userId, int roomId, RoomModel roomModel, CancellationToken cancellationToken = default);
    Task DeleteRoomAsync(int userId, int roomId, CancellationToken cancellationToken = default);
    Task JoinRoomAsync(int userId, int roomId, CancellationToken cancellationToken = default);
    Task LeaveRoomAsync(int userId, int roomId, CancellationToken cancellationToken = default);

    Task<IEnumerable<RoomModel>> GetAllUsersRoomsAsync(int userId, CancellationToken cancellationToken = default);


}