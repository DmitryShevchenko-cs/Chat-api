using AutoMapper;
using Chat.BLL.Exceptions;
using Chat.BLL.Services.Interfaces;
using Chat.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace Chat.Web.Hubs;

[AllowAnonymous]
public class RoomHub(IUserService userService, IRoomService roomService, IMessageService messageService, IMapper mapper) : Hub
{
    
    public async Task SendMessage(int userId, int roomId, string textMess)
    {
        var messageModel = await messageService.CreateMessageAsync(userId, roomId, textMess);
        await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", 
            JsonSerializer.Serialize(mapper.Map<MessageViewModel>(messageModel)));
    }
    
    public override async Task OnConnectedAsync()
    {
        try
        {
            var roomIdValues = Context.GetHttpContext()!.Request.Query["roomId"];
            var userIdValues = Context.GetHttpContext()!.Request.Query["userId"];
            
            if (string.IsNullOrEmpty(roomIdValues))
            {
                throw new ArgumentException("RoomId or UserId is missing");
            }

            if (int.TryParse(roomIdValues[0], out int roomId) &&
                int.TryParse(userIdValues[0], out int userId)
                )
            {
                var room = await roomService.GetByIdAsync(roomId);
                if (room == null)
                {
                    throw new RoomNotFoundException($"Room with Id {roomId} not found");
                }

                await roomService.JoinRoomAsync(userId, roomId);
                
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
            }
            else
            {
                throw new ArgumentException("Invalid RoomId or UserId");
            }
            
            await base.OnConnectedAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message); 
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var roomIdValues = Context.GetHttpContext()!.Request.Query["roomId"];
        var userIdValues = Context.GetHttpContext()!.Request.Query["userId"];
        if (!string.IsNullOrEmpty(roomIdValues) 
            && int.TryParse(roomIdValues[0], out int roomId) &&
            int.TryParse(userIdValues[0], out int userId))
        {
            var room = await roomService.GetByIdAsync(roomId);
            if (room == null)
            {
                throw new RoomNotFoundException($"Room with Id {roomId} not found");
            }
            
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }
        await base.OnDisconnectedAsync(exception);
    }
    
}