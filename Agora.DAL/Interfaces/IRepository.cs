using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Agora.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAll();
        Task<T> Get(int id);
        Task Create(T item);
        void Update(T item);
        Task Delete(int id);

        //дефолтный метод
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate) => Task.FromResult<IEnumerable<T>>(null);
    }
}

