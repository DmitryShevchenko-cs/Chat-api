using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.DAL.Repositories;

public class RoomRepository(ChatDbContext chatDbContext) : IRoomRepository
{
    private readonly ChatDbContext _chatDbContext = chatDbContext;
    
    public IQueryable<Room> GetAll()
    {
        return _chatDbContext.Rooms.AsQueryable();
    }

    public async Task<Room?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _chatDbContext.Rooms
            .Include(r => r.Creator)
            .Include(r => r.Users)
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Room> CreateAsync(Room entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = await _chatDbContext.Rooms.AddAsync(entity, cancellationToken);
        await _chatDbContext.SaveChangesAsync(cancellationToken);
        return entityEntry.Entity;
    }

    public async Task<Room> EditAsync(Room entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = _chatDbContext.Rooms.Update(entity);
        await _chatDbContext.SaveChangesAsync(cancellationToken);
        return entityEntry.Entity;
    }

    public async Task DeleteAsync(Room entity, CancellationToken cancellationToken = default)
    {
        _chatDbContext.Rooms.Remove(entity);
        await _chatDbContext.SaveChangesAsync(cancellationToken);
    }
}