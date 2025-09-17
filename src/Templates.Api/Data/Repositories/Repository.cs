using Microsoft.EntityFrameworkCore;
using Templates.Api.Data.Entities;

namespace Templates.Api.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
            => await _dbSet.FindAsync([id], cancellationToken);

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
            => await _dbSet.ToListAsync(cancellationToken);

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
