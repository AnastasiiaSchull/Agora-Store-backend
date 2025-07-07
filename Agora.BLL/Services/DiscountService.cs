using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<DiscountDTO>> GetAll()
        {
            var discounts = await Database.Discounts.GetAll();
            var list = await discounts.ToListAsync(); 
            return _mapper.Map<IEnumerable<DiscountDTO>>(list);
        }

        public async Task<IEnumerable<DiscountDTO>> GetActiveDiscounts()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var discounts = await Database.Discounts.Find(d => d.StartDate <= today && d.EndDate >= today);
            return _mapper.Map<IEnumerable<DiscountDTO>>(discounts);
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
                AllProducts = discount.AllProducts
            };
        }
  
        public async Task Create(DiscountDTO discountDTO)
        {
            var discount = _mapper.Map<Discount>(discountDTO);
            // привязка брендов
            if (discountDTO.Brands?.Any() == true)
            {
                discount.Brands = new List<Brand>();
                foreach (var brandDTO in discountDTO.Brands)
                {
                    var brand = await Database.Brands.Get(brandDTO.Id);
                    if (brand != null)
                    {
                        discount.Brands.Add(brand);
                    }
                }
            }
            // привязка категорий
            if (discountDTO.Categories?.Any() == true)
            {
                discount.Categories = new List<Category>();
                foreach (var categoryDTO in discountDTO.Categories)
                {
                    var category = await Database.Categories.Get(categoryDTO.Id);
                    if (category != null)
                    {
                        discount.Categories.Add(category);
                    }
                }
            }
            // привязка подкатегорий
            if (discountDTO.Subcategories?.Any() == true)
            {
                discount.Subcategories = new List<Subcategory>();
                foreach (var subcategoryDTO in discountDTO.Subcategories)
                {
                    var subcategory = await Database.Subcategories.Get(subcategoryDTO.Id);
                    if (subcategory != null)
                    {
                        discount.Subcategories.Add(subcategory);
                    }
                }
            }
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
                AllProducts = discountDTO.AllProducts
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
