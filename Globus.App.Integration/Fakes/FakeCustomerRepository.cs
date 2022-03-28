using Globus.App.Data.Entities;
using Globus.App.Data.Repositories;
using Globus.App.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globus.App.Integration.Tests.Fakes
{
    internal class FakeCustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public FakeCustomerRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public List<Customer> PageCustomers(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
