
using Templates.Api.Data.Entities;

namespace Templates.Api.Data.Repositories
{
    public class TemplatesRepository : Repository<Template>, ITemplatesRepository
    {
        public TemplatesRepository(AppDbContext context) : base(context) { }
    }
}
