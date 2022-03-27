using Globus.App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globus.App.Data.Contexts
{
    public class GlobusContext : DbContext
    {
        public GlobusContext(DbContextOptions<GlobusContext> options):base(options)
        {

        }

        public DbSet<OTP> OTPs { get; set; }
    }
}
