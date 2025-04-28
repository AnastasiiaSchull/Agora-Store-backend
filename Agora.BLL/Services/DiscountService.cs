using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using AutoMapper;
using Agora.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Services
{
    public class DiscountService : IDiscountService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public DiscountService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<DiscountDTO>> GetAll()
        {
            var discounts = await Database.Discounts.GetAll();
            return _mapper.Map<IQueryable<DiscountDTO>>(discounts.ToList());
        }

        public async Task<DiscountDTO> Get(int id)
        {
            var discount = await Database.Discounts.Get(id);
            if (discount == null)
                throw new ValidationExceptionFromService("There is no discount with this id", "");
            return new DiscountDTO
            {
                Id = discount.Id,
                Name = discount.Name,
                Percentage = discount.Percentage,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                Type = discount.Type,
            };
        }

        public async Task Create(DiscountDTO discountDTO)
        {
            var discount = new Discount
            {
                Id = discountDTO.Id,
                Name = discountDTO.Name,
                Percentage = discountDTO.Percentage,
                StartDate = discountDTO.StartDate,
                EndDate = discountDTO.EndDate,
                Type = discountDTO.Type,
            };
            await Database.Discounts.Create(discount);
            await Database.Save();
        }
        public async Task Update(DiscountDTO discountDTO)
        {
            var discount = new Discount
            {
                Id = discountDTO.Id,
                Name = discountDTO.Name,
                Percentage = discountDTO.Percentage,
                StartDate = discountDTO.StartDate,
                EndDate = discountDTO.EndDate,
                Type = discountDTO.Type,
            };
            Database.Discounts.Update(discount);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Discounts.Delete(id);
            await Database.Save();
        }
    }
}
