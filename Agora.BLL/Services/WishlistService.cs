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

        public async Task<IQueryable<WishlistDTO>> GetAll()
        {
            var wishlists = await Database.Wishlists.GetAll();
            return _mapper.Map<IQueryable<WishlistDTO>>(wishlists.ToList());
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
                DateAdded = wishlistDTO.DateAdded
            };
            await Database.Wishlists.Create(wishlist);
            await Database.Save();
        }
        public async Task Update(WishlistDTO wishlistDTO)
        {

            var wishlist = new Wishlist
            {
                Id = wishlistDTO.Id,
                DateAdded = wishlistDTO.DateAdded
            };
            Database.Wishlists.Update(wishlist);
            await Database.Save();
        }
        public async Task Delete(int id)
        {
            await Database.Wishlists.Delete(id);
            await Database.Save();
        }
    }
}
