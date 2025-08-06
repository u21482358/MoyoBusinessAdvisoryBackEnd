using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// https://stackoverflow.com/questions/77565815/entityframeworkcore-add-migration-the-module-entityframeworkcore-could-not-b


// https://www.c-sharpcorner.com/UploadFile/abhikumarvatsa/code-first-approach-in-entity-framework/
namespace MoyoBusinessAdvisory.Models
{
    // When running enable migration ... https://stackoverflow.com/questions/19475998/no-context-type-found-in-the-assembly-asp-net-mvc4
    public class DataContext: IdentityDbContext<AppUser>
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SchoolDb;Trusted_Connection=True;");
        //}
        //https://medium.com/@nwonahr/configuring-many-to-many-relationship-in-entity-framework-core-698dd356abec
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        //public DbSet<Client> Clients { get; set; }
        public DbSet<ProductOrder> Orders { get; set; }
       // public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<AppUser> Users { get; set; }
       // public DbSet<Orderline> Orderlines { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

       public DbSet<VendorProduct> VendorProducts { get; set; }

        public DbSet<Product> Products { get; set; }
        
        public DbSet<Vendor> Vendors { get; set; }
    }

   // public override void OnModelCreating()
}
