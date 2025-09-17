using Microsoft.EntityFrameworkCore;
using Templates.Api.Data.Entities;

namespace Templates.Api.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _context;

        public UsersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
            => await _context.Users.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
            => await _context.Users.AsNoTracking().ToListAsync(cancellationToken);

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
            => await _context.Users.AnyAsync(u => u.Id == id, cancellationToken);
    }
}
