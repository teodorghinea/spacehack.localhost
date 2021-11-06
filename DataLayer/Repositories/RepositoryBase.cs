using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public interface IRepositoryBase<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync(bool asNoTracking = false, bool includeDeleted = false);
        Task<T> GetByIdAsync(Guid id, bool asNoTracking = false, bool includeDeleted = false);
        void Insert(T record);
        void InsertRange(IList<T> records);
        void Update(T record);
        void UpdateRange(IList<T> records);
        void Delete(T record);
        void DeleteRange(IList<T> records);
    }

    public class RepositoryBase<T> : IDisposable, IRepositoryBase<T> where T : BaseEntity, new()
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        protected RepositoryBase(EfDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(bool asNoTracking = false, bool includeDeleted = false)
        {
            return await GetRecords(asNoTracking, includeDeleted).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id, bool asNoTracking = false, bool includeDeleted = false)
        {
            return await GetRecords(asNoTracking, includeDeleted)
                .FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public void Insert(T record)
        {
            if (_context.Entry(record).State == EntityState.Detached)
            {
                _context.Attach(record);
                _context.Entry(record).State = EntityState.Added;
            }
        }

        public void InsertRange(IList<T> records)
        {
            foreach (var record in records)
            {
                Insert(record);
            }
        }

        public void Update(T record)
        {
            if (_context.Entry(record).State == EntityState.Detached)
                _context.Attach(record);

            _context.Entry(record).State = EntityState.Modified;
        }

        public void UpdateRange(IList<T> records)
        {
            foreach (var record in records)
            {
                Update(record);
            }
        }

        public void Delete(T record)
        {
            if (record != null)
            {
                record.DeletedAt = DateTime.UtcNow;
                Update(record);
            }
        }

        public void DeleteRange(IList<T> records)
        {
            foreach (var record in records)
            {
                Delete(record);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        protected IQueryable<T> GetRecords(bool asNoTracking = false, bool includeDeleted = false)
        {
            var query = includeDeleted
                ? _dbSet.Where(entity => entity.DeletedAt != null)
                : _dbSet.Where(entity => entity.DeletedAt == null);

            query = asNoTracking ? query.AsNoTracking() : query;

            return query;
        }
    }
}
