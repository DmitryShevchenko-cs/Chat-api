using Chat.BLL.Exceptions;
using Chat.BLL.Models;
using Chat.BLL.Services;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Repositories;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Chat.Test.Services;

public class UserServiceTest: DefaultServiceTest<IUserService, UserService>
{
    protected override void SetUpAdditionalDependencies(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        base.SetUpAdditionalDependencies(services);
    }
    
    [Test]
    public async Task CreateUser_Success()
    {
        var user = await Service.CreateUserAsync("Full Name");
        Assert.That(user.FullName, Is.EqualTo("Full Name"));
    }
    
    [Test]
    public Task GetUserById_UserNotFound()
    {
        Assert.ThrowsAsync<UserNotFoundException>(async() => await Service.GetByIdAsync(0));
        return Task.CompletedTask;
    }
    
    [Test]
    public async Task UpdateUserName_Success()
    {
       var user = await Service.CreateUserAsync("Full Name");
       const string name = "Updated Name";
       await Service.UpdateUserAsync(user.Id, new UserModel
       {
           FullName = name
       });
       var updatedUser = await Service.GetByIdAsync(user.Id);
       Assert.That(updatedUser!.FullName, Is.EqualTo(name));
    }
    
}