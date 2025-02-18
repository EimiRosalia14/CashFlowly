using CashFlowly.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Infrastructure.Persistence.Contexts
{
    public class CashFlowlyDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

        public CashFlowlyDbContext(DbContextOptions<CashFlowlyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
        }
    }
}
