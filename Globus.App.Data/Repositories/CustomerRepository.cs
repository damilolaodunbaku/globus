using Globus.App.Data.Contexts;
using Globus.App.Data.Entities;
using Globus.App.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Globus.App.Data.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(GlobusContext dbContext)
            : base(dbContext)
        {
        }

        public GlobusContext GlobusContext { get { return dbContext as GlobusContext; } }

        public List<Customer> PageCustomers (int pageNumber,int pageSize)
        {
            return GlobusContext.Customer
                    .Skip(pageNumber * pageSize)
                    .Take(pageSize)
                    .ToList();
        }
    }
}
