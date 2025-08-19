using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly AgoraContext db;

        public WishlistRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Wishlist>> GetAll()
        {
            return db.Wishlists
                .Include(w => w.ProductWishlists)
                    .ThenInclude(pw => pw.Product);
        }

        public async Task<Wishlist> Get(int id)
        {
            return await db.Wishlists.FindAsync(id);
        }

        public async Task Create(Wishlist wishlist)
        {
            await db.Wishlists.AddAsync(wishlist);
        }

        public void Update(Wishlist wishlist)
        {
            db.Entry(wishlist).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Wishlist? wishlist = await db.Wishlists.FindAsync(id);
            if (wishlist != null)
                db.Wishlists.Remove(wishlist);
        }

        public async Task<Wishlist?> GetWithProducts(int id)
        {
            return await db.Wishlists
                .Include(w => w.ProductWishlists)
                    .ThenInclude(pw => pw.Product)
                .FirstOrDefaultAsync(w => w.Id == id);
        }
        public async Task<List<Wishlist>> GetByCustomerId(int customerId)
        {
            return await db.Wishlists
                .Where(w => w.Customer.Id == customerId)
                .Include(w => w.Customer)
                .Include(w => w.ProductWishlists)
                    .ThenInclude(pw => pw.Product)
                .ToListAsync();
        }

    }

}
