using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Agora.DAL.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Agora.BLL.Services
{
    public class WishlistService : IWishlistService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public WishlistService(IUnitOfWork database, IMapper mapper)
        {
            Database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WishlistDTO>> GetAll()
        {
            var query = await Database.Wishlists.GetAll();
            var wishlists = await query
                .Include(w => w.ProductWishlists)
                    .ThenInclude(pw => pw.Product)
                .ToListAsync();

            return _mapper.Map<IEnumerable<WishlistDTO>>(wishlists);
        }

        public async Task<WishlistDTO> Get(int id)
        {
            var wishlist = await Database.Wishlists.Get(id);
            if (wishlist == null)
                throw new ValidationExceptionFromService("There is no user with this id", "");

            return new WishlistDTO
            {
                Id = wishlist.Id,
                Name = wishlist.Name,
                DateAdded = wishlist.DateAdded,
                Customer = wishlist.CustomerId.HasValue
                ? new CustomerDTO { Id = wishlist.CustomerId.Value }
                : null,
                ProductIds = wishlist.ProductWishlists?.Select(pw => pw.ProductId).ToList() ?? new List<int>()
            };
        }

        public async Task<WishlistDTO> Create(WishlistDTO wishlistDTO)
        {
            var wishlist = new Wishlist
            {
                DateAdded = wishlistDTO.DateAdded ?? DateOnly.FromDateTime(DateTime.Today),
                CustomerId = wishlistDTO.Customer.Id,
                Name = wishlistDTO.Name,
                ProductWishlists = new List<ProductWishlist>()
            };

            if (wishlistDTO.ProductIds != null && wishlistDTO.ProductIds.Any())
            {
                foreach (var productId in wishlistDTO.ProductIds)
                {
                    var product = await Database.Products.Get(productId);
                    if (product == null)
                        throw new ValidationExceptionFromService($"Product with ID {productId} not found", "");

                    wishlist.ProductWishlists.Add(new ProductWishlist
                    {
                        ProductId = product.Id,
                        Wishlist = wishlist
                    });
                }
            }

            await Database.Wishlists.Create(wishlist);
            await Database.Save();

            // Возвращаем созданный объект в виде DTO
            return new WishlistDTO
            {
                Id = wishlist.Id,
                Name = wishlist.Name,
                DateAdded = wishlist.DateAdded,
                Customer = wishlist.CustomerId.HasValue
                ? new CustomerDTO { Id = wishlist.CustomerId.Value }
                : null,
                ProductIds = wishlist.ProductWishlists?.Select(pw => pw.ProductId).ToList() ?? new List<int>()
            };
        }


        public async Task Update(WishlistDTO wishlistDTO)
        {
            var wishlist = await Database.Wishlists.GetWithProducts(wishlistDTO.Id);
            if (wishlist == null)
                throw new ValidationExceptionFromService("Wishlist not found", "");

            wishlist.DateAdded = wishlistDTO.DateAdded ?? DateOnly.FromDateTime(DateTime.Today);

            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Wishlists.Delete(id);
            await Database.Save();
        }

        public async Task AddProductToWishlist(int wishlistId, int productId)
        {
            var wishlist = await Database.Wishlists.GetWithProducts(wishlistId);
            if (wishlist == null)
                throw new ValidationExceptionFromService("Wishlist not found", "");

            var product = await Database.Products.Get(productId);
            if (product == null)
                throw new ValidationExceptionFromService("Product not found", "");

            wishlist.ProductWishlists ??= new List<ProductWishlist>();
            if (!wishlist.ProductWishlists.Any(pw => pw.ProductId == productId))
            {
                wishlist.ProductWishlists.Add(new ProductWishlist
                {
                    WishlistId = wishlistId,
                    ProductId = productId,
                    DateAdded = DateTime.Now
                });
            }            

            Database.Wishlists.Update(wishlist);
            await Database.Save();
        }

        public async Task RemoveProductFromWishlist(int wishlistId, int productId)
        {
            var wishlist = await Database.Wishlists.GetWithProducts(wishlistId);
            if (wishlist == null)
                throw new ValidationExceptionFromService("Wishlist not found", "");

            var toRemove = wishlist.ProductWishlists?.FirstOrDefault(pw => pw.ProductId == productId);
            if (toRemove != null)
            {
                wishlist.ProductWishlists!.Remove(toRemove);
                Database.Wishlists.Update(wishlist);
                await Database.Save();
            }
        }

        public async Task<WishlistDTO> GetWithProducts(int id)
        {
            var wishlist = await Database.Wishlists.GetWithProducts(id);

            if (wishlist == null)
                throw new ValidationExceptionFromService("Wishlist not found", "Id");

            return _mapper.Map<WishlistDTO>(wishlist);
        }

        public async Task<IEnumerable<WishlistDTO>> GetByCustomerId(int customerId)
        {
            var wishlists = await Database.Wishlists.GetByCustomerId(customerId);
            return _mapper.Map<IEnumerable<WishlistDTO>>(wishlists);
        }
    }
}
