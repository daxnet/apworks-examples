using Apworks.Examples.CustomerService.EntityFramework.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apworks.Examples.CustomerService.EntityFramework
{
    public class CustomerDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .ToTable("Customers")
                .HasKey(x => x.Id);
            modelBuilder.Entity<Customer>()
                .Property(x => x.Id)
                .ForSqlServerHasDefaultValueSql("newid()");
            modelBuilder.Entity<Customer>()
                .Property(x => x.Name)
                .IsUnicode()
                .IsRequired()
                .HasMaxLength(20);
            modelBuilder.Entity<Customer>()
                .Property(x => x.Email)
                .IsUnicode()
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Address>()
                .ToTable("Addresses")
                .HasKey(x => x.Id);
            modelBuilder.Entity<Address>()
                .Property(x => x.Id)
                .ForSqlServerHasDefaultValueSql("newid()");

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\sqlexpress; Database=CustomerService; Integrated Security=SSPI;");
        }
    }
}
