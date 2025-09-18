using Templates.Api.Data.Entities;

namespace Templates.Api.Data.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
