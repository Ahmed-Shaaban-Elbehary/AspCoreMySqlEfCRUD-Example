using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class DbContainer:DbContext
    {
        public DbContainer(DbContextOptions<DbContainer> options):base(options)
        {}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>().HasKey(m => m.Id);
            base.OnModelCreating(builder);
        }
        public DbSet<Customer> customers { get; set; }
    }
}
