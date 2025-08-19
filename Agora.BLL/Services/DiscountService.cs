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
            var discount = await Database.Discounts.GetWithRelations(id);
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
                AllProducts = discount.AllProducts,

                //включаем связанные сущности
                Brands = discount.Brands?.Select(b => new BrandDTO { Id = b.Id, Name = b.Name }).ToList(),
                Categories = discount.Categories?.Select(c => new CategoryDTO { Id = c.Id, Name = c.Name }).ToList(),
                Subcategories = discount.Subcategories?.Select(s => new SubcategoryDTO { Id = s.Id, Name = s.Name }).ToList(),
                Products = discount.Products?.Select(p => new ProductDTO { Id = p.Id, Name = p.Name }).ToList()
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
            var existingDiscount = await Database.Discounts.GetWithRelations(discountDTO.Id);
            if (existingDiscount == null)
                throw new ValidationExceptionFromService("Discount not found", "");

            // обновляем основные поля
            existingDiscount.Name = discountDTO.Name;
            existingDiscount.Percentage = discountDTO.Percentage;
            existingDiscount.StartDate = discountDTO.StartDate;
            existingDiscount.EndDate = discountDTO.EndDate;
            existingDiscount.Type = discountDTO.Type;
            existingDiscount.AllProducts = discountDTO.AllProducts;

            // очищаем текущие связи
            existingDiscount.Brands?.Clear();
            existingDiscount.Categories?.Clear();
            existingDiscount.Subcategories?.Clear();
            existingDiscount.Products?.Clear();

            // заново устанавливаем связи

            if (discountDTO.Brands?.Any() == true)
            {
                existingDiscount.Brands = new List<Brand>();
                foreach (var brandDTO in discountDTO.Brands)
                {
                    var brand = await Database.Brands.Get(brandDTO.Id);
                    if (brand != null)
                        existingDiscount.Brands.Add(brand);
                }
            }

            if (discountDTO.Categories?.Any() == true)
            {
                existingDiscount.Categories = new List<Category>();
                foreach (var catDTO in discountDTO.Categories)
                {
                    var cat = await Database.Categories.Get(catDTO.Id);
                    if (cat != null)
                        existingDiscount.Categories.Add(cat);
                }
            }

            if (discountDTO.Subcategories?.Any() == true)
            {
                existingDiscount.Subcategories = new List<Subcategory>();
                foreach (var subDTO in discountDTO.Subcategories)
                {
                    var sub = await Database.Subcategories.Get(subDTO.Id);
                    if (sub != null)
                        existingDiscount.Subcategories.Add(sub);
                }
            }

            if (discountDTO.Products?.Any() == true)
            {
                existingDiscount.Products = new List<Product>();
                foreach (var prodDTO in discountDTO.Products)
                {
                    var prod = await Database.Products.Get(prodDTO.Id);
                    if (prod != null)
                        existingDiscount.Products.Add(prod);
                }
            }

            Database.Discounts.Update(existingDiscount);
            await Database.Save();
        }
        public async Task Delete(int id)
        {
            var discount = await Database.Discounts.GetWithRelations(id);
            if (discount != null)
            {
                discount.Brands?.Clear();
                discount.Categories?.Clear();
                discount.Subcategories?.Clear();
                discount.Products?.Clear();

                Database.Discounts.Delete(discount);
                await Database.Save();
            }
        }
    }
}

