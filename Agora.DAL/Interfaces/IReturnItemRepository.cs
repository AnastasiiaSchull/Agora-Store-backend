using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IReturnItemRepository
    {
        Task<IQueryable<ReturnItem>> GetAll();
        //Task<IQueryable<ReturnItem>> GetNewReturns(int storeId);
        Task<IQueryable<ReturnItem>> GetAllByStore(int storeId);
        //Task<IQueryable<ReturnItem>> GetAllByCustomer(int customerId);
        Task<IQueryable<ReturnItem>> GetFilteredReturns(int storeId, string field, string value);
        Task<ReturnItem> Get(int id);
        Task Create(ReturnItem item);
        void Update(ReturnItem item);
        Task Delete(int id);

    }
}
