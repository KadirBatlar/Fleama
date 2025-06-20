using Fleama.Core.Entities;
using System.Linq.Expressions;

namespace Fleama.Service.Abstract
{
    public interface IService<T> where T : class, IEntity, new()
    {
        List<T> GetAll();
        List<T> GetAll(Expression<Func<T, bool>> expression);
        IQueryable<T> GetQueryable();
        T Get(Expression<Func<T, bool>> expression);
        T FindById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        int SaveChanges();

        //async methods
        Task<T> FindByIdAsync(int id);
        Task<T> GetAsync (Expression<Func<T, bool>> expression);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task<int> SaveChangesAsync();        
    }
}