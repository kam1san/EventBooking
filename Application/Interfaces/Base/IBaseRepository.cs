namespace Application.Interfaces.Base
{
    public interface IBaseRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T ev);
        Task SaveChangesAsync();
    }
}
