#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WarriorSalesAPI.Models;

namespace WarriorSalesAPI.Data
{
    public class WarriorSalesAPIContext : DbContext
    {
        public WarriorSalesAPIContext (DbContextOptions<WarriorSalesAPIContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WarriorSalesAPI.Models.Product>().Property(p => p.Id).ValueGeneratedOnAdd().UseIdentityAlwaysColumn();
        }

        public DbSet<WarriorSalesAPI.Models.Product> Product { get; set; }
    }
}
