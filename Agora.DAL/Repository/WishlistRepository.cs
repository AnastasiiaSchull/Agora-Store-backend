using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class WishlistRepository: IRepository<Wishlist>
    {
        private AgoraContext db;
        public WishlistRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Wishlist>> GetAll()
        {
            return db.Wishlists;
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
    }
}
