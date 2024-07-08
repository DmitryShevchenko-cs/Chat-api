using AutoMapper;
using Chat.BLL.Services.Interfaces;
using Chat.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Web.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class RoomController(IRoomService roomService, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateRoom(RoomCreateModel room, CancellationToken cancellationToken)
    {
        var createdRoom = await roomService.CreateRoomAsync(room.CreatorId, room.Name, cancellationToken);
        return Ok(mapper.Map<RoomViewModel>(createdRoom));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteRoom([FromQuery] int userId, [FromQuery] int roomId,
        CancellationToken cancellationToken)
    {
        await roomService.DeleteRoomAsync(userId, roomId, cancellationToken);
        return Ok();
    }

    [HttpPut("join")]
    public async Task<IActionResult> JoinRoom([FromQuery] int userId, [FromQuery] int roomId, CancellationToken cancellationToken)
    {
        await roomService.JoinRoomAsync(userId, roomId, cancellationToken);
        return Ok();
    }
    [HttpPut("leave")]
    public async Task<IActionResult> LeaveRoom([FromQuery] int userId, [FromQuery] int roomId, CancellationToken cancellationToken)
    {
        await roomService.LeaveRoomAsync(userId, roomId, cancellationToken);
        return Ok();
    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetRooms(int userId, CancellationToken cancellationToken)
    {
        var rooms = await roomService.GetAllRoomsAsync(userId, cancellationToken);
        return Ok(mapper.Map<List<RoomViewModel>>(rooms));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetByName([FromQuery]string roomName, CancellationToken cancellationToken)
    {
        var rooms = await roomService.GetByNameRoomsAsync(roomName, cancellationToken);
        return Ok(mapper.Map<List<RoomViewModel>>(rooms));
    }
    
}