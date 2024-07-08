using Chat.BLL.Exceptions;
using Chat.BLL.Models;
using Chat.BLL.Services;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Repositories;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Chat.Test.Services;

public class RoomServiceTest : DefaultServiceTest<IRoomService, RoomService>
{
    protected override void SetUpAdditionalDependencies(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IRoomService, RoomService>();
        base.SetUpAdditionalDependencies(services);
    }

    [Test]
    public async Task CreateRoom_Access()
    {
        var userService = ServiceProvider.GetRequiredService<IUserService>();
        var user = await userService.CreateUserAsync("Full Name");
        var room = await Service.CreateRoomAsync(user.Id, "Test Room" );
        Assert.That(room.Creator.FullName, Is.EqualTo(user.FullName));
    }
    
    [Test]
    public async Task JoinRoom_Access()
    {
        var userService = ServiceProvider.GetRequiredService<IUserService>();
        var user = await userService.CreateUserAsync("Full Name");
        var room = await Service.CreateRoomAsync(user.Id, "Test Room" );
        Assert.That(room.Creator.FullName, Is.EqualTo(user.FullName));
        
        var member1 = await userService.CreateUserAsync("member1");
        var member2 = await userService.CreateUserAsync("member2");
        var member3 = await userService.CreateUserAsync("member3");

        await Service.JoinRoomAsync(member1.Id, room.Id);
        await Service.JoinRoomAsync(member2.Id, room.Id);
        await Service.JoinRoomAsync(member3.Id, room.Id);

        var roomUpdated = await Service.GetByIdAsync(room.Id);
        Assert.That(roomUpdated!.Users.Count(), Is.EqualTo(4));
    }

    [Test]
    public async Task LeaveRoom_Access()
    {
        var userService = ServiceProvider.GetRequiredService<IUserService>();
        var user = await userService.CreateUserAsync("Full Name");
        var room = await Service.CreateRoomAsync(user.Id, "Test Room" );
        Assert.That(room.Creator.FullName, Is.EqualTo(user.FullName));
        
        var roomUpdated = await Service.GetByIdAsync(room.Id);
        Assert.That(roomUpdated!.Users.Count(), Is.EqualTo(1));
        
        var member1 = await userService.CreateUserAsync("member1");
        await Service.JoinRoomAsync(member1.Id, room.Id);
        roomUpdated = await Service.GetByIdAsync(room.Id);
        Assert.That(roomUpdated!.Users.Count(), Is.EqualTo(2));
        
        //leave by member1
        await Service.LeaveRoomAsync(member1.Id, room.Id);
        roomUpdated = await Service.GetByIdAsync(room.Id);
        Assert.That(roomUpdated!.Users.Count(), Is.EqualTo(1));
        
        //try to leave by creator
        Assert.ThrowsAsync<NoPermissionsException>(async() => await Service.LeaveRoomAsync(user.Id, room.Id));
    }

    [Test]
    public async Task DeleteRoom_Access()
    {
        var userService = ServiceProvider.GetRequiredService<IUserService>();
        var user = await userService.CreateUserAsync("Full Name");
        var room = await Service.CreateRoomAsync(user.Id, "Test Room" );
        Assert.That(room.Creator.FullName, Is.EqualTo(user.FullName));
        
        var member1 = await userService.CreateUserAsync("member1");
        var member2 = await userService.CreateUserAsync("member2");
        var member3 = await userService.CreateUserAsync("member3");

        await Service.JoinRoomAsync(member1.Id, room.Id);
        await Service.JoinRoomAsync(member2.Id, room.Id);
        await Service.JoinRoomAsync(member3.Id, room.Id);

        var roomUpdated = await Service.GetByIdAsync(room.Id);
        Assert.That(roomUpdated!.Users.Count(), Is.EqualTo(4));

        await Service.DeleteRoomAsync(user.Id, room.Id);
        Assert.ThrowsAsync<RoomNotFoundException>(async() => await Service.GetByIdAsync(user.Id));
    }
    
    [Test]
    public async Task DeleteRoom_NoPermission_Access()
    {
        var userService = ServiceProvider.GetRequiredService<IUserService>();
        var user = await userService.CreateUserAsync("Full Name");
        var room = await Service.CreateRoomAsync(user.Id, "Test Room" );
        Assert.That(room.Creator.FullName, Is.EqualTo(user.FullName));
        
        var member1 = await userService.CreateUserAsync("member1");
        await Service.JoinRoomAsync(member1.Id, room.Id);
        
        Assert.ThrowsAsync<NoPermissionsException>(async() => await Service.DeleteRoomAsync(member1.Id, room.Id));
    }
    
    
    [Test]
    public async Task UpdateRoom_Access()
    {
        const string newName = "new Name";
        
        var userService = ServiceProvider.GetRequiredService<IUserService>();
        var user = await userService.CreateUserAsync("Full Name");
        var room = await Service.CreateRoomAsync(user.Id, "Test Room" );
        Assert.That(room.Creator.FullName, Is.EqualTo(user.FullName));
        
        var room2 = await Service.UpdateRoomAsync(user.Id, room.Id, new RoomModel
        {
            Name = newName,
        });

        var updatedRoom = await Service.GetByIdAsync(room.Id);
        Assert.That(updatedRoom!.Name, Is.EqualTo(newName));
    }
    
}