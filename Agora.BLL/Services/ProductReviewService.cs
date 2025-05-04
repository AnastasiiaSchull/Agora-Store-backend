using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Agora.BLL.Services
{
    public class ProductReviewService : IProductReviewService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public ProductReviewService(IUnitOfWork database, IMapper mapper)
        {
            Database = database;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<ProductReviewDTO>> GetAll()
        {
            var query = await Database.ProductReviews.GetAll();

            var productReviews = await query
                .Include(r => r.Customer)
                    .ThenInclude(c => c.User)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductReviewDTO>>(productReviews);
        }

        public async Task<IEnumerable<ProductReviewDTO>> GetReviewsByStoreId(int storeId)
        {
            var query = await Database.ProductReviews.GetAll();

            var productReviews = await query
                .Where(pr => pr.Product != null && pr.Product.StoreId == storeId)
                .Include(pr => pr.Product)
                .Include(pr => pr.Customer)
                    .ThenInclude(c => c.User)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductReviewDTO>>(productReviews);
        }

        public async Task<IEnumerable<ProductReviewDTO>> GetFilteredReviewsByStoreId(int storeId, string field, string value)
        {
            var query = await Database.ProductReviews.GetFilteredReviews(storeId, field, value);

            var filteredReviews = await query
                .Include(r => r.Customer)
                    .ThenInclude(c => c.User)
                .Include(r => r.Product)  
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductReviewDTO>>(filteredReviews);
        }


        public async Task<ProductReviewDTO> Get(int id)
        {
            var productReview = await Database.ProductReviews.Get(id);
            if (productReview == null)
                throw new ValidationExceptionFromService("There is no product review with this id", "");
            return new ProductReviewDTO
            {
                Id = productReview.Id,
                Comment = productReview.Comment,
                Rating = productReview.Rating,
                Date = productReview.Date,
                ProductId = productReview.ProductId
            };
        }

        public async Task Create(ProductReviewDTO productReviewDTO)
        {
            var productReview = new ProductReview
            {
                Comment = productReviewDTO.Comment,
                Rating = productReviewDTO.Rating,
                Date = productReviewDTO.Date,
                ProductId = productReviewDTO.ProductId
            };
            await Database.ProductReviews.Create(productReview);
            await Database.Save();
        }
        public async Task Update(ProductReviewDTO productReviewDTO)
        {
            var productReview = new ProductReview
            {
                Id = productReviewDTO.Id,
                Comment = productReviewDTO.Comment,
                Rating = productReviewDTO.Rating,
                Date = productReviewDTO.Date,
                ProductId = productReviewDTO.ProductId
            };
            Database.ProductReviews.Update(productReview);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.ProductReviews.Delete(id);
            await Database.Save();
        }
    }
}
