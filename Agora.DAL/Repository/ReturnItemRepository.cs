using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Agora.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Agora.DAL.Repository
{
    public class ReturnItemRepository: IReturnItemRepository
    {
        private AgoraContext db;
        public ReturnItemRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<ReturnItem>> GetAll()
        {
            return db.ReturnItems;
        }

        public async Task<IQueryable<ReturnItem>> GetAllByStore(int storeId)
        {
            return db.ReturnItems.Where(o => o.Product.Store.Id == storeId).OrderByDescending(o => o.Return.ReturnDate);
        }
        public async Task<IQueryable<ReturnItem>> GetNewReturns(int storeId)
        {
            return db.ReturnItems.Where(o => o.Product.Store.Id == storeId && o.Return.Status == Enums.ReturnStatus.Requested);
        }

        public async Task<IQueryable<ReturnItem>> GetFilteredReturns(int storeId, string field, string value)
        {
            if (field == "status")
            {
                var matchedStatuses = Enum.GetValues(typeof(ReturnStatus))
                    .Cast<ReturnStatus>()
                    .Where(status => status.ToString().ToLower().Contains(value.ToLower()))
                    .ToList();

                return db.ReturnItems
                    .Include(ri => ri.Return)
                    .Where(ri => ri.Return != null && matchedStatuses.Contains(ri.Return.Status))
                    .OrderByDescending(ri => ri.Return.ReturnDate);
            }
            else if (field == "date")
            {
                if (DateOnly.TryParse(value, out var parsedDate))
                {
                    return db.ReturnItems
                        .Include(ri => ri.Return)
                        .Where(ri => ri.Return != null && ri.Return.ReturnDate == parsedDate)
                        .OrderByDescending(ri => ri.Return.ReturnDate);
                }
                else
                {
                    return Enumerable.Empty<ReturnItem>().AsQueryable();
                }
            }

            return null;
        }

        public async Task<ReturnItem> Get(int id)
        {
            return await db.ReturnItems
                .Include(ri => ri.Return)
                .Include(ri => ri.Product)
                .Include(ri => ri.OrderItem)
                .FirstOrDefaultAsync(ri => ri.Id == id);
        }

        public async Task Create(ReturnItem returnItem)
        {
            await db.ReturnItems.AddAsync(returnItem);
        }

        public void Update(ReturnItem returnItem)
        {
            db.Entry(returnItem).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            ReturnItem? returnItem = await db.ReturnItems.FindAsync(id);
            if (returnItem != null)
                db.ReturnItems.Remove(returnItem);
        }
    }
}
