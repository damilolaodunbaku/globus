using Globus.App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globus.App.Integration.Tests.Contexts
{
    public class GlobusContext : DbContext
    {
        public GlobusContext(DbContextOptions<GlobusContext> options): base(options)
        {

        }
        public DbSet<OTP> OTPs { get; set; }
        public DbSet<Customer> Customer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.EmailAddress)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.PhoneNumber)
                .IsUnique();
        }
    }
}
