using Fleama.Core.Entities;
using Fleama.Data;
using Fleama.Service.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fleama.Service.Concrete
{
    public class BaseService<T> : IBaseService<T> where T : class, IEntity, new()
    {
        internal DatabaseContext _context;
        internal DbSet<T> _dbSet;

        public BaseService(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet?.Remove(entity);
        }

        public T FindById(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefault(expression);
        }

        public List<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public List<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).AsNoTracking().ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).AsNoTracking().ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.AppUsers.CountAsync();
        }
    }
}