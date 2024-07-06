namespace Chat.DAL.Repositories.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();

    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    
    public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task<TEntity> EditAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}