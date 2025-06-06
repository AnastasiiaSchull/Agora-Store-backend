using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Agora.BLL.Services
{
    public class SellerReviewService : ISellerReviewService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public SellerReviewService(IUnitOfWork database, IMapper mapper)
        {
            Database = database;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SellerReviewDTO>> GetAll()
        {
            var query = await Database.SellerReviews.GetAll(); // теперь query — IQueryable<SellerReview>

            var sellerReviews = await query
                .Include(r => r.Customer)
                    .ThenInclude(c => c.User)
                .ToListAsync();

            return _mapper.Map<IEnumerable<SellerReviewDTO>>(sellerReviews);
        }

        public async Task<SellerReviewDTO> Get(int id)
        {
            var sellerReview = await Database.SellerReviews.Get(id);
            if (sellerReview == null)
                throw new ValidationExceptionFromService("There is no seller review with this id", "");
            return new SellerReviewDTO
            {
                Id = sellerReview.Id,
                Comment = sellerReview.Comment,
                Rating = sellerReview.Rating,
                Date = sellerReview.Date,
                SellerId = sellerReview.SellerId,
                CustomerId = sellerReview.CustomerId
            };
        }

        public async Task Create(SellerReviewDTO sellerReviewDTO)
        {
            var sellerReview = new SellerReview
            {
                Comment = sellerReviewDTO.Comment,
                Rating = sellerReviewDTO.Rating,
                Date = sellerReviewDTO.Date ?? DateOnly.FromDateTime(DateTime.Today),
                SellerId = sellerReviewDTO.SellerId,
                CustomerId = sellerReviewDTO.CustomerId
            };
            await Database.SellerReviews.Create(sellerReview);
            await Database.Save();

            // После добавления отзыва, пересчёт рейтинга продавца
            if (sellerReviewDTO.SellerId.HasValue)
            {
                await UpdateSellerRating(sellerReviewDTO.SellerId.Value);
            }
        }
        private async Task UpdateSellerRating(int sellerId)
        {
            var allReviews = await Database.SellerReviews.GetAll();
            var reviews = allReviews.Where(r => r.SellerId == sellerId).ToList();

            if (reviews.Count == 0) return;

            var averageRating = (float)Math.Round(reviews.Average(r => r.Rating), 2);

            var seller = await Database.Sellers.Get(sellerId);
            if (seller != null)
            {
                seller.Rating = averageRating;
                Database.Sellers.Update(seller);
                await Database.Save();
            }
        }

        public async Task Update(SellerReviewDTO sellerReviewDTO)
        {
            var sellerReview = new SellerReview
            {
                Id = sellerReviewDTO.Id,
                Comment = sellerReviewDTO.Comment,
                Rating = sellerReviewDTO.Rating,
                Date = sellerReviewDTO.Date ?? DateOnly.FromDateTime(DateTime.Today),
                SellerId = sellerReviewDTO.SellerId
            };
            Database.SellerReviews.Update(sellerReview);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.SellerReviews.Delete(id);
            await Database.Save();
        }
    }
}
