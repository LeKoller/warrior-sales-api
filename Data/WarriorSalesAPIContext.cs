#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WarriorSalesAPI.Data
{
    public class WarriorSalesAPIContext : DbContext
    {
        public WarriorSalesAPIContext (DbContextOptions<WarriorSalesAPIContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Product>()
                .Property(p => p.Id).ValueGeneratedOnAdd().UseIdentityAlwaysColumn();

            modelBuilder.Entity<Models.SaleItem>()
                .Property(si => si.Id).ValueGeneratedOnAdd().UseIdentityAlwaysColumn();

            modelBuilder.Entity<Models.Order>()
                .Property(o => o.Id).ValueGeneratedOnAdd().UseIdentityAlwaysColumn();
            modelBuilder.Entity<Models.Order>()
                .Property(o => o.Creation).ValueGeneratedOnAdd();
            modelBuilder.Entity<Models.Order>()
                .Property(o => o.Delivery).ValueGeneratedOnUpdateSometimes();

            modelBuilder.Entity<Models.Team>()
                .Property(t => t.Id).ValueGeneratedOnAdd().UseIdentityAlwaysColumn();
        }

        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.SaleItem> SaleItems { get; set; }
        public DbSet<Models.Order> Orders { get; set; }
        public DbSet<Models.Team> Teams { get; set; }
    }
}
