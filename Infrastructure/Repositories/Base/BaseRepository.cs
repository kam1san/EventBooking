using Application.Interfaces.Base;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext context;
        protected readonly DbSet<T> dbSet;

        public BaseRepository(AppDbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public virtual async Task<List<T>> GetAllAsync() =>
            await dbSet.ToListAsync();

        public virtual async Task<T?> GetByIdAsync(Guid id) =>
            await dbSet.FindAsync(id);

        public virtual async Task AddAsync(T entity) =>
            await dbSet.AddAsync(entity);

        public async Task SaveChangesAsync() =>
            await context.SaveChangesAsync();
    }
}
