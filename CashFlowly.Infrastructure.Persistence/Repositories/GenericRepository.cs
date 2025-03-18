using CashFlowly.Core.Application.Interfaces.Repositories;
using CashFlowly.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
    where T : class
    {
        protected readonly CashFlowlyDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(CashFlowlyDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<(List<T> Result, int TotalCount,
            int TotalPages, bool HasPrevious, bool HasNext)>
            GetAllOrderAndPaginateAsync(Expression<Func<T, bool>> searchPredicate = null,
            Expression<Func<T, object>> orderBy = null,
            bool isDescending = false,
            int? pageNumber = null,
            int? pageSize = null,
            params Expression<Func<T, object>>[] properties)
        {
            IQueryable<T> query = _dbSet;
            if (searchPredicate != null)
                query = query.Where(searchPredicate);
            if (orderBy != null)
                query = !isDescending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            foreach (var property in properties)
            {
                query = query.Include(property);
            }

            int totalNumber = await query.CountAsync();

            if ((pageNumber.HasValue && pageNumber > 0) && (pageSize.HasValue && pageSize > 0))
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            int? totalPages = (pageNumber.HasValue && pageNumber > 0) && (pageSize.HasValue && pageSize > 0)
                ? (int?)Math.Ceiling((double)totalNumber / pageSize.Value)
                : null;
            bool? hasPrevious = pageNumber.HasValue ? pageNumber > 1 : null;
            bool? hasNext = pageNumber.HasValue ? pageNumber < totalPages : null;
            var result = await query.ToListAsync();
            return (result, totalNumber, totalPages ?? 0, hasPrevious ?? false, hasNext ?? false);
        }
        public async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<List<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[] properties)
        {
            var query = _dbSet.AsQueryable();
            foreach (var property in properties)
            {
                query = query.Include(property);
            }
            return await query.ToListAsync();
        }
        
        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> match) => await _dbSet.Where(match).ToListAsync();
        public async Task<T> FindAsync(Expression<Func<T, bool>> match) => await _dbSet.FirstOrDefaultAsync(match);

        public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<T> UpdateAsync(T entity, int id)
        {
            try
            {
                var entry = await _context.Set<T>().FindAsync(id);
                _context.Entry(entry).CurrentValues.SetValues(entity);


                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IDbTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction().GetDbTransaction();
        }


    }
}
