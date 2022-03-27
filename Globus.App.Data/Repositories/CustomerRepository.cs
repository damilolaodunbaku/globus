using Globus.App.Data.Contexts;
using Globus.App.Data.Entities;

namespace Globus.App.Data.Repositories
{
    public class CustomerRepository : Repository<Customer>
    {
        public CustomerRepository(GlobusContext dbContext)
            : base(dbContext)
        {
        }

        public GlobusContext GlobusContext { get { return dbContext as GlobusContext; } }
    }
}
