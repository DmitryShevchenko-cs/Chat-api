using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.DAL.Repositories;

public class RoomRepository(ChatDbContext chatDbContext) : IRoomRepository
{
    public IQueryable<Room> GetAll()
    {
        return chatDbContext.Rooms.AsQueryable();
    }

    public async Task<Room?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await chatDbContext.Rooms
            .Include(r => r.Creator)
            .Include(r => r.Users)
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Room> CreateAsync(Room entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = await chatDbContext.Rooms.AddAsync(entity, cancellationToken);
        await chatDbContext.SaveChangesAsync(cancellationToken);
        return entityEntry.Entity;
    }

    public async Task<Room> UpdateAsync(Room entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = chatDbContext.Rooms.Update(entity);
        await chatDbContext.SaveChangesAsync(cancellationToken);
        return entityEntry.Entity;
    }

    public async Task DeleteAsync(Room entity, CancellationToken cancellationToken = default)
    {
        chatDbContext.Rooms.Remove(entity);
        await chatDbContext.SaveChangesAsync(cancellationToken);
    }
}