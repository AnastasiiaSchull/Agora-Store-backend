using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
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
            var wishlists =  await query
                            .Include(w => w.Products) // если Products нужен    
                              .ToListAsync();
            return _mapper.Map<IEnumerable<WishlistDTO>>(wishlists);
        }
        
        public async Task<WishlistDTO> Get(int id)
        {
            var wishlist = await Database.Wishlists.Get(id);
            if(wishlist == null)
                throw new ValidationExceptionFromService("There is no user with this id", "");
            return new WishlistDTO
            {
                Id = wishlist.Id,
                DateAdded = wishlist.DateAdded
            };
        }

        public async Task Create(WishlistDTO wishlistDTO)
        {
            var wishlist = new Wishlist
            {
                DateAdded = wishlistDTO.DateAdded ?? DateOnly.FromDateTime(DateTime.Today),
                CustomerId = wishlistDTO.Customer.Id,
                Products = new List<Product>()
            };

            
            if (wishlistDTO.ProductIds != null && wishlistDTO.ProductIds.Any())
            {
                foreach (var productId in wishlistDTO.ProductIds)
                {
                    var product = await Database.Products.Get(productId);
                    if (product == null)
                        throw new ValidationExceptionFromService($"Product with ID {productId} not found", "");
                                        
                    if (!wishlist.Products.Any(p => p.Id == productId))
                        wishlist.Products.Add(product);
                }
            }

            await Database.Wishlists.Create(wishlist);
            await Database.Save();
        }
        public async Task Update(WishlistDTO wishlistDTO)
        {

            var wishlist = new Wishlist
            {
                Id = wishlistDTO.Id,
                DateAdded = wishlistDTO.DateAdded ?? DateOnly.FromDateTime(DateTime.Today)
            };
            Database.Wishlists.Update(wishlist);
            await Database.Save();
        }
        public async Task Delete(int id)
        {
            await Database.Wishlists.Delete(id);
            await Database.Save();
        }

        public async Task AddProductToWishlist(int wishlistId, int productId)
        {
            var wishlist = await Database.Wishlists.Get(wishlistId);
            if (wishlist == null)
                throw new ValidationExceptionFromService("Wishlist not found", "");

            var product = await Database.Products.Get(productId);
            if (product == null)
                throw new ValidationExceptionFromService("Product not found", "");

            wishlist.Products ??= new List<Product>();
            if (!wishlist.Products.Any(p => p.Id == productId))
                wishlist.Products.Add(product);

            Database.Wishlists.Update(wishlist);
            await Database.Save();
        }

        public async Task RemoveProductFromWishlist(int wishlistId, int productId)
        {
            var wishlist = await Database.Wishlists.Get(wishlistId);
            if (wishlist == null)
                throw new ValidationExceptionFromService("Wishlist not found", "");

            var productToRemove = wishlist.Products?.FirstOrDefault(p => p.Id == productId);
            if (productToRemove != null)
            {
                wishlist.Products!.Remove(productToRemove);
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

    }
}
