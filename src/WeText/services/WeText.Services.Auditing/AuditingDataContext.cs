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
        public DbSet<EventItem> EventItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<EventItem>()
                .ToTable("EventItems")
                .HasKey(x => x.Id);
            modelBuilder.Entity<EventItem>()
                .Property(x => x.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            modelBuilder.Entity<EventItem>()
                .Property(x => x.Intent)
                .IsUnicode()
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder.Entity<EventItem>()
                .Property(x => x.Payload)
                .IsUnicode()
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"User ID=wetext;Password=wetext;Host=192.168.0.111;Port=5432;Database=wetext_auditing;");
        }
    }
}
