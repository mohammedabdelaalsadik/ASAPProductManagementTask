namespace ASAPTaskAPI.Infrastructure.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<TEntity?> FirstOrDefaultAsync(Func<TEntity, bool> predicate);
        Task SaveChangesAsync();
    }
}
