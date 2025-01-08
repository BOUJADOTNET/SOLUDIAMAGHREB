using System.Linq.Expressions;

namespace SOLUDIAMAGHREB.Services.Contract
{
    public interface IRepository<T> where T : class
    {
        IEnumerator<T> GetAll(Expression<Func<T, bool>>? filter = null);
        T GetByID(string id);
        void Add(T entity);
        void Remove(T entity);
        void AddRange(IEnumerable<T> entities);
        void RemoveRange(IEnumerable<T> entities);
    }
}
