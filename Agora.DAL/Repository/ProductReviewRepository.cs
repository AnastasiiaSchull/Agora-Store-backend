using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class ProductReviewRepository: IRepository<ProductReview>
    {
        private AgoraContext db;
        public ProductReviewRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<ProductReview>> GetAll()
        {
            return db.ProductReviews;
        }

        public async Task<ProductReview> Get(int id)
        {
            return await db.ProductReviews.FindAsync(id);
        }

        public async Task Create(ProductReview productReview)
        {
            await db.ProductReviews.AddAsync(productReview);
        }

        public void Update(ProductReview productReview)
        {
            db.Entry(productReview).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            ProductReview? productReview = await db.ProductReviews.FindAsync(id);
            if (productReview != null)
                db.ProductReviews.Remove(productReview);
        }
    }
}
