using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    public class SellerService : ISellerService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public SellerService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<SellerDTO>> GetAll()
        {
            var sellers = await Database.Sellers.GetAll();
            var sellerList = sellers.ToList();

            var sellerDTOs = sellerList.Select(s => new SellerDTO
            {
                Id = s.Id,
                UserId = s.UserId,
                Name = s.User?.Name,
                Surname = s.User?.Surname
            });

            return sellerDTOs.AsQueryable();
        }

        public async Task<List<int>> GetAllSellerIds()
        {
            var sellers = await Database.Sellers.GetAll();
            return sellers.Select(s => s.Id).ToList();

        }
        public async Task<SellerDTO> Get(int id)
        {
            var seller = await Database.Sellers.Get(id);
            if (seller == null)
                throw new ValidationExceptionFromService("There is no seller with this id", "");
            return new SellerDTO
            {
                Id = seller.Id,
                Rating = seller.Rating,
                UserId = seller.UserId
            };
        }

        public async Task<int> Create(SellerDTO sellerDTO)
        {
            var seller = new Seller
            {
                Rating = sellerDTO.Rating,
                UserId = sellerDTO.UserId
            };
            await Database.Sellers.Create(seller);
            await Database.Save();

            return seller.Id;
        }
        public async Task Update(SellerDTO sellerDTO)
        {

            var seller = new Seller
            {
                Id = sellerDTO.Id,
                Rating = sellerDTO.Rating,
                UserId = sellerDTO.UserId
            };
            Database.Sellers.Update(seller);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Sellers.Delete(id);
            await Database.Save();
        }

    }
}

