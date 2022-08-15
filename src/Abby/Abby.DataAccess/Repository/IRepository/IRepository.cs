using System.Linq.Expressions;

namespace Abby.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity); // Add new entity
        void Remove(T entity); // Remove entity
        void RemoveRange(IEnumerable<T> entities); // Remove a range of entities
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderby = null,
            string? includeProperties = null); // Get all entities
        T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null); // Get the first entity that matches the filter or else return the default
    }
}
