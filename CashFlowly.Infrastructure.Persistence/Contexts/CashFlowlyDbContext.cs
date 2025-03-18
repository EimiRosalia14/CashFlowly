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
        public DbSet<CategoriaIngresoPersonalizada> CategoriasIngresosPersonalizadas { get; set; }
        public DbSet<CategoriaGastoPersonalizada> CategoriasGastosPersonalizadas { get; set; }
        public DbSet<MetaFinanciera> MetasFinancieras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Índice único para Email
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Relación Usuario -> Gastos (SIN DELETE CASCADE)
            modelBuilder.Entity<Gasto>()
                .HasOne(g => g.Usuario)
                .WithMany(u => u.Gastos)
                .HasForeignKey(g => g.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction); // Cambia CASCADE a NO ACTION

            // Relación Usuario -> Ingresos (SIN DELETE CASCADE)
            modelBuilder.Entity<Ingreso>()
                .HasOne(i => i.Usuario)
                .WithMany(u => u.Ingresos)
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction); // Cambia CASCADE a NO ACTION

            modelBuilder.Entity<Ingreso>()
                .HasOne(i => i.CategoriaPersonalizada)
                .WithMany()
                .HasForeignKey(i => i.CategoriaPersonalizadaId)
                .OnDelete(DeleteBehavior.SetNull); // Permitir NULL si se elimina la categoría personalizada

            // Relación Usuario -> Cuentas (SIN DELETE CASCADE)
            modelBuilder.Entity<Cuenta>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Cuentas)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction); // Cambia CASCADE a NO ACTION

            // Relación Usuario -> Metas (SIN DELETE CASCADE)
            modelBuilder.Entity<MetaFinanciera>()
                .HasOne(m => m.Usuario)
                .WithMany(u => u.Metas)
                .HasForeignKey(m => m.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction); // Cambia CASCADE a NO ACTION

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

            // Agregar Categorías Fijas de Ingresos
            modelBuilder.Entity<CategoriaIngreso>().HasData(
                new CategoriaIngreso { Id = 1, Nombre = "Salario" },
                new CategoriaIngreso { Id = 2, Nombre = "Deudas cobradas" },
                new CategoriaIngreso { Id = 3, Nombre = "Regalos / Donaciones" },
                new CategoriaIngreso { Id = 4, Nombre = "Ventas personales" },
                new CategoriaIngreso { Id = 5, Nombre = "Inversiones" }
            );

            // Agregar Categorías Fijas de Gastos
            modelBuilder.Entity<CategoriaGasto>().HasData(
                new CategoriaGasto { Id = 1, Nombre = "Vivienda" },
                new CategoriaGasto { Id = 2, Nombre = "Alimentación" },
                new CategoriaGasto { Id = 3, Nombre = "Transporte" },
                new CategoriaGasto { Id = 4, Nombre = "Salud" },
                new CategoriaGasto { Id = 5, Nombre = "Entretenimiento" }
            );
        }
    }
}
