using System.Linq;

namespace PhotoProspector.Domain.Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
