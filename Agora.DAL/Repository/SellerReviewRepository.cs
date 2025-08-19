using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class SellerReviewRepository : IRepository<SellerReview>
    {
        private AgoraContext db;
        public SellerReviewRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<SellerReview>> GetAll()
        {
            return db.SellerReviews;
        }

        public async Task<SellerReview> Get(int id)
        {
            return await db.SellerReviews.FindAsync(id);
        }

        public async Task Create(SellerReview sellerReview)
        {
            await db.SellerReviews.AddAsync(sellerReview);
        }

        public void Update(SellerReview sellerReview)
        {
            db.Entry(sellerReview).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            SellerReview? sellerReview = await db.SellerReviews.FindAsync(id);
            if (sellerReview != null)
                db.SellerReviews.Remove(sellerReview);
        }
    }
}
