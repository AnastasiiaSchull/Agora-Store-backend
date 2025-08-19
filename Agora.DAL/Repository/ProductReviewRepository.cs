using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class ProductReviewRepository: IProductReviewRepository
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

        public async Task<IQueryable<ProductReview>> GetFilteredReviews(int storeId, string field, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Enumerable.Empty<ProductReview>().AsQueryable();
            }

            IQueryable<ProductReview> query = db.ProductReviews
                .Include(r => r.Product)
                .Include(r => r.Customer)
                .Where(r => r.Product != null && r.Product.StoreId == storeId);

            switch (field.ToLower())
            {
                case "name":
                    query = query.Where(review => review.Product.Name != null &&
                                                  review.Product.Name.ToLower().Contains(value.ToLower()));
                    break;

                case "date":
                    if (DateOnly.TryParse(value, out var parsedDate))
                    {
                        query = query.Where(review => review.Date == parsedDate);
                    }
                    else
                    {
                        return Enumerable.Empty<ProductReview>().AsQueryable();
                    }
                    break;

                case "rating":
                    if (decimal.TryParse(value, out var parsedRating))
                    {
                        query = query.Where(review => review.Rating == parsedRating);
                    }
                    else
                    {
                        return Enumerable.Empty<ProductReview>().AsQueryable();
                    }
                    break;

                case "customer":
                    query = query.Where(review =>
                        review.Customer != null &&
                        ((review.Customer.User.Surname != null && review.Customer.User.Surname.ToLower().Contains(value.ToLower()))));
                    break;

                default:
                    return Enumerable.Empty<ProductReview>().AsQueryable(); // unsupported field
            }

            return query.OrderByDescending(r => r.Date);
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
