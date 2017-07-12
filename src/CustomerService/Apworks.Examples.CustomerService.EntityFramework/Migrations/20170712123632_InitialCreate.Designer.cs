using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Apworks.Examples.CustomerService.EntityFramework;

namespace Apworks.Examples.CustomerService.EntityFramework.Migrations
{
    [DbContext(typeof(CustomerDbContext))]
    [Migration("20170712123632_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Apworks.Examples.CustomerService.EntityFramework.Model.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("State");

                    b.Property<string>("Street");

                    b.Property<string>("ZipCode");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Apworks.Examples.CustomerService.EntityFramework.Model.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<Guid?>("ContactAddressId");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.HasIndex("ContactAddressId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Apworks.Examples.CustomerService.EntityFramework.Model.Customer", b =>
                {
                    b.HasOne("Apworks.Examples.CustomerService.EntityFramework.Model.Address", "ContactAddress")
                        .WithMany()
                        .HasForeignKey("ContactAddressId");
                });
        }
    }
}
