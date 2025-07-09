using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Agora.DAL.Repository
{
    public class SellerRepository: ISellerRepository
    {
        private AgoraContext db;
        public SellerRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Seller>> GetAll()
        {
            return db.Sellers.Include(s => s.User);
        }

        public async Task<Seller> Get(int id)
        {
            return await db.Sellers.FindAsync(id);
        }
        public async Task<Seller> GetByUserId(int id)
        {
            return await db.Sellers.FirstOrDefaultAsync(a => a.UserId == id);
        }

        public async Task Create(Seller seller)
        {
            await db.Sellers.AddAsync(seller);
        }

        public void Update(Seller seller)
        {
            db.Entry(seller).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Seller? seller = await db.Sellers.FindAsync(id);
            if (seller != null)
                db.Sellers.Remove(seller);
        }

        public async Task<Seller?> FindWithIncludes(
            Expression<Func<Seller, bool>> predicate,
            Func<IQueryable<Seller>, IIncludableQueryable<Seller, object>> include)
        {
            var query = db.Sellers.AsQueryable();
            query = include(query);
            return await query.FirstOrDefaultAsync(predicate);
        }

    }
}
