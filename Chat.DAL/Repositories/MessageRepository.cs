using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.DAL.Repositories;

public class MessageRepository(ChatDbContext chatDbContext) : IMessageRepository
{
    public IQueryable<Message> GetAll()
    {
        return chatDbContext.Messages.AsQueryable();
    }

    public async Task<Message?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await chatDbContext.Messages
            .Include(r => r.User)
            .Include(r => r.Room)
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Message> CreateAsync(Message entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = await chatDbContext.Messages.AddAsync(entity, cancellationToken);
        await chatDbContext.SaveChangesAsync(cancellationToken);
        return entityEntry.Entity;
    }

    public async Task<Message> UpdateAsync(Message entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = chatDbContext.Messages.Update(entity);
        await chatDbContext.SaveChangesAsync(cancellationToken);
        return entityEntry.Entity;
    }

    public async Task DeleteAsync(Message entity, CancellationToken cancellationToken = default)
    {
        chatDbContext.Messages.Remove(entity);
        await chatDbContext.SaveChangesAsync(cancellationToken);
    }
}