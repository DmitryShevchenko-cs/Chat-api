using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.DAL.Repositories;

public class UserRepository(ChatDbContext chatDbContext) : IUserRepository
{
    private readonly ChatDbContext _chatDbContext = chatDbContext;

    public IQueryable<User> GetAll()
    {
        return _chatDbContext.Users.AsQueryable();
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _chatDbContext.Users
            .Include(r => r.CreatedRooms)
            .Include(r => r.JoinedRooms)
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<User> CreateAsync(User entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = await _chatDbContext.Users.AddAsync(entity, cancellationToken);
        await _chatDbContext.SaveChangesAsync(cancellationToken);
        return entityEntry.Entity;
    }

    public async Task<User> EditAsync(User entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = _chatDbContext.Users.Update(entity);
        await _chatDbContext.SaveChangesAsync(cancellationToken);
        return entityEntry.Entity;
    }

    public async Task DeleteAsync(User entity, CancellationToken cancellationToken = default)
    {
        _chatDbContext.Users.Remove(entity);
        await _chatDbContext.SaveChangesAsync(cancellationToken);
    }
}