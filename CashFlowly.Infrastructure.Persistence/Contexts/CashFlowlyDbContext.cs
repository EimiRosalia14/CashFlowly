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

        public CashFlowlyDbContext(DbContextOptions<CashFlowlyDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<CategoriaIngreso> CategoriasIngresos { get; set; }
        public DbSet<CategoriaGasto> CategoriasGastos { get; set; }
        public DbSet<MetaFinanciera> MetasFinancieras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Índice único para Email
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Relaciones de Usuario
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Cuentas)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Ingresos)
                .WithOne(i => i.Usuario)
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Gastos)
                .WithOne(g => g.Usuario)
                .HasForeignKey(g => g.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de Gasto
            modelBuilder.Entity<Gasto>()
                .HasOne(g => g.Categoria)
                .WithMany(c => c.Gastos)
                .HasForeignKey(g => g.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Gasto>()
                .HasOne(g => g.Cuenta)
                .WithMany(c => c.Gastos)
                .HasForeignKey(g => g.CuentaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de Ingreso
            modelBuilder.Entity<Ingreso>()
                .HasOne(i => i.Categoria)
                .WithMany(c => c.Ingresos)
                .HasForeignKey(i => i.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ingreso>()
                .HasOne(i => i.Cuenta)
                .WithMany(c => c.Ingresos)
                .HasForeignKey(i => i.CuentaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de precisión en campos decimales para evitar truncamientos
            modelBuilder.Entity<Cuenta>()
                .Property(c => c.SaldoDisponible)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Gasto>()
                .Property(g => g.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Ingreso>()
                .Property(i => i.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MetaFinanciera>()
                .Property(m => m.Objetivo)
                .HasPrecision(18, 2);
        }
    }
}
