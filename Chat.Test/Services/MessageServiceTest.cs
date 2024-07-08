using Chat.BLL.Services;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Repositories;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Chat.Test.Services;

public class MessageServiceTest : DefaultServiceTest<IMessageService, MessageService>
{
    protected override void SetUpAdditionalDependencies(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IRoomService, RoomService>();
        
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IMessageService, MessageService>();
        base.SetUpAdditionalDependencies(services);
    }

    [Test]
    public async Task CreateMessage_Success()
    {
        var userService = ServiceProvider.GetRequiredService<IUserService>();
        var roomService = ServiceProvider.GetRequiredService<IRoomService>();
        var user = await userService.CreateUserAsync("Full Name");
        var room = await roomService.CreateRoomAsync(user.Id, "Test Room" );
        Assert.That(room.Creator.FullName, Is.EqualTo(user.FullName));
        
        var member1 = await userService.CreateUserAsync("member1");
        await roomService.JoinRoomAsync(member1.Id, room.Id);

        var message1 = await Service.CreateMessageAsync(user.Id, room.Id, "test user mess1");
        var message2 = await Service.CreateMessageAsync(member1.Id, room.Id, "test member mess1");
        var messages = await Service.GetRoomMessagesAsync(member1.Id, room.Id);
        
        Assert.That(messages.Select(r => r.Text).Contains(message1.Text));
        Assert.That(messages.Select(r => r.Text).Contains(message2.Text));
        
    }
}