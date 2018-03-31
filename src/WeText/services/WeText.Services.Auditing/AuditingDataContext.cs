using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeText.Services.Auditing.Models;

namespace WeText.Services.Auditing
{
    public class AuditingDataContext : DbContext
    {
        public DbSet<Authentication> Authentications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Authentication>()
                .ToTable("Authentications")
                .HasKey(x => x.Id);
            modelBuilder.Entity<Authentication>()
                .Property(x => x.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            modelBuilder.Entity<Authentication>()
                .Property(x => x.AccountName)
                .IsUnicode()
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<Authentication>()
                .Property(x => x.FailReason)
                .IsUnicode()
                .IsRequired()
                .HasMaxLength(1024);

            modelBuilder.Entity<Authentication>()
                .Property(x => x.Succeeded);

            modelBuilder.Entity<Authentication>()
                .Property(x => x.TimeOfAuthentication);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"User ID=wetext;Password=wetext;Host=localhost;Port=5432;Database=wetext_auditing;");
        }
    }
}
