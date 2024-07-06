using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.DAL.Repositories;

public class MessageRepository(ChatDbContext chatDbContext) : IMessageRepository
{
    private readonly ChatDbContext _chatDbContext = chatDbContext;
    
    public IQueryable<Message> GetAll()
    {
        return _chatDbContext.Messages.AsQueryable();
    }

    public async Task<Message?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _chatDbContext.Messages
            .Include(r => r.User)
            .Include(r => r.Room)
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Message> CreateAsync(Message entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = await _chatDbContext.Messages.AddAsync(entity, cancellationToken);
        await _chatDbContext.SaveChangesAsync(cancellationToken);
        return entityEntry.Entity;
    }

    public async Task<Message> EditAsync(Message entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = _chatDbContext.Messages.Update(entity);
        await _chatDbContext.SaveChangesAsync(cancellationToken);
        return entityEntry.Entity;
    }

    public async Task DeleteAsync(Message entity, CancellationToken cancellationToken = default)
    {
        _chatDbContext.Messages.Remove(entity);
        await _chatDbContext.SaveChangesAsync(cancellationToken);
    }
}