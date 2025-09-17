using Templates.Api.Data.Entities;

namespace Templates.Api.Data.Repositories
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public UsersRepository(AppDbContext context) : base(context) { }
    }
}
    