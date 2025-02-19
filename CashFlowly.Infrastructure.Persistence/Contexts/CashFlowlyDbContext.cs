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
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<MetaFinanciera> MetasFinancieras { get; set; }
        public DbSet<Alerta> Alertas { get; set; }
        public DbSet<MensajeChatbot> MensajesChatbot { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Restringir el correo a único en la BD
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();

            // Relaciones entre entidades
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Transacciones)
                .WithOne(t => t.Usuario)
                .HasForeignKey(t => t.UsuarioId);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.MetasFinancieras)
                .WithOne(m => m.Usuario)
                .HasForeignKey(m => m.UsuarioId);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Alertas)
                .WithOne(a => a.Usuario)
                .HasForeignKey(a => a.UsuarioId);

            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.Categoria)
                .WithMany(c => c.Transacciones)
                .HasForeignKey(t => t.CategoriaId);

            // Corrección: Configurar la precisión de los valores `decimal`
            modelBuilder.Entity<Usuario>()
                .Property(u => u.SaldoDisponible)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaccion>()
                .Property(t => t.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MetaFinanciera>()
                .Property(m => m.MontoObjetivo)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MetaFinanciera>()
                .Property(m => m.MontoAhorrado)
                .HasPrecision(18, 2);
        }
    }
}
