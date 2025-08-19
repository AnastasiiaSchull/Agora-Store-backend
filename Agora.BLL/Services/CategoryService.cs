using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Agora.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public CategoryService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var categories = await Database.Categories.GetAll();
            return _mapper.Map<IQueryable<Category>, IEnumerable<CategoryDTO>>(categories);
        }
      
        public async Task<CategoryDTO> Get(int id)
        {
            var category = await Database.Categories.Get(id);
            if (category == null)
                throw new ValidationExceptionFromService("There is no category with this id", "");
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public async Task Create(CategoryDTO categoryDTO)
        {
            var category = new Category
            {
                //Id = categoryDTO.Id,
                Name= categoryDTO.Name,
            };
            await Database.Categories.Create(category);
            await Database.Save();
        }

        public async Task Update(CategoryDTO categoryDTO)
        {
            var category = new Category
            {
                Id = categoryDTO.Id,
                Name = categoryDTO.Name,
            };
            Database.Categories.Update(category);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            var category = await Database.Categories.Get(id);

            var relatedProducts = await Database.Products.Find(p => p.CategoryId == id);
            if (relatedProducts.Any())
                throw new ValidationExceptionFromService("Cannot delete category because it is linked to products.", "");

            var relatedSubcategories = await Database.Subcategories.Find(s => s.CategoryId == id);
            if (relatedSubcategories.Any())
                throw new ValidationExceptionFromService("Cannot delete category because it is linked to subcategories.", "");

            await Database.Categories.Delete(id);
            await Database.Save();
        }
    }
}
