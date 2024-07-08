using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.DAL.Repositories;

public class UserRepository(ChatDbContext chatDbContext) : IUserRepository
{
    public IQueryable<User> GetAll()
    {
        return chatDbContext.Users
            .Include(r => r.CreatedRooms)
            .Include(r => r.JoinedRooms)
            .AsQueryable();
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await chatDbContext.Users
            .Include(r => r.CreatedRooms)
            .Include(r => r.JoinedRooms)
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<User> CreateAsync(User entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = await chatDbContext.Users.AddAsync(entity, cancellationToken);
        await chatDbContext.SaveChangesAsync(cancellationToken);
        return entityEntry.Entity;
    }

    public async Task<User> UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = chatDbContext.Users.Update(entity);
        await chatDbContext.SaveChangesAsync(cancellationToken);
        return entityEntry.Entity;
    }

    public async Task DeleteAsync(User entity, CancellationToken cancellationToken = default)
    {
        chatDbContext.Users.Remove(entity);
        await chatDbContext.SaveChangesAsync(cancellationToken);
    }
}