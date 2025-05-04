using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IProductReviewRepository
    {
        Task<IQueryable<ProductReview>> GetAll();
        Task<IQueryable<ProductReview>> GetFilteredReviews(int storeId, string field, string value);
        Task<ProductReview> Get(int id);
        Task Create(ProductReview item);
        void Update(ProductReview item);
        Task Delete(int id);

    }
}
