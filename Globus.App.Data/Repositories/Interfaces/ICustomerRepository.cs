using Globus.App.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globus.App.Data.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        List<Customer> PageCustomers(int pageNumber, int pageSize);
    }
}
