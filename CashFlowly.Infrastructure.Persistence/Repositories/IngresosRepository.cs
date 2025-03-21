using CashFlowly.Core.Application.Interfaces.Repositories;
using CashFlowly.Core.Domain.Entities;
using CashFlowly.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Infrastructure.Persistence.Repositories
{
    public class IngresosRepository : IIngresosRepository
    {
        private readonly CashFlowlyDbContext _context;

        public IngresosRepository(CashFlowlyDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarIngresoAsync(Ingreso ingreso)
        {
            await _context.Ingresos.AddAsync(ingreso);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Ingreso>> ObtenerIngresosPorUsuarioAsync(int usuarioId)
        {
            return await _context.Ingresos
                .Where(i => i.UsuarioId == usuarioId)
                .Include(i => i.Cuenta)
                .Include(i => i.Categoria)
                .Include(i => i.CategoriaPersonalizada)  // Se incluye la categoría personalizada
                .ToListAsync();
        }

        public async Task<Ingreso> ObtenerIngresoPorIdAsync(int ingresoId)
        {
            return await _context.Ingresos
                .Include(i => i.Cuenta)
                .Include(i => i.Categoria)
                .Include(i => i.CategoriaPersonalizada)  // Se incluye la categoría personalizada
                .FirstOrDefaultAsync(i => i.Id == ingresoId);
        }

        public async Task EditarIngresoAsync(Ingreso ingreso)
        {
            var ingresoExistente = await _context.Ingresos.FindAsync(ingreso.Id);
            if (ingresoExistente != null)
            {
                ingresoExistente.Monto = ingreso.Monto;
                ingresoExistente.Fecha = ingreso.Fecha;
                ingresoExistente.CuentaId = ingreso.CuentaId;
                ingresoExistente.CategoriaId = ingreso.CategoriaId;
                ingresoExistente.CategoriaPersonalizadaId = ingreso.CategoriaPersonalizadaId; // Nombre corregido

                await _context.SaveChangesAsync();
            }
        }

        public async Task EliminarIngresoAsync(int ingresoId)
        {
            var ingreso = await _context.Ingresos.FindAsync(ingresoId);
            if (ingreso != null)
            {
                _context.Ingresos.Remove(ingreso);
                await _context.SaveChangesAsync();
            }
        }
    }
}