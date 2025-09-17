using Templates.Api.Data.Entities;

namespace Templates.Api.Data.Repositories
{
    public interface IUsersRepository
    {
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(User user, CancellationToken cancellationToken);
        Task UpdateAsync(User user, CancellationToken cancellationToken);
        Task DeleteAsync(User user, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    }
}
