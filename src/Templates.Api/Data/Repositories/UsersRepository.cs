using Microsoft.EntityFrameworkCore;
using Templates.Api.Data.Entities;

namespace Templates.Api.Data.Repositories
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public UsersRepository(AppDbContext context) : base(context) { }

        public override async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(u => u.Address)
                .ToListAsync(cancellationToken);
        }

        public override async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
    }
}
