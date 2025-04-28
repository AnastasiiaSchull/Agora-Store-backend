using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using AutoMapper;

namespace Agora.BLL.Services
{
    public class SubcategoryService : ISubcategoryService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public SubcategoryService(IUnitOfWork database, IMapper mapper)
        {
            Database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubcategoryDTO>> GetAll()
        {
            var subcategories = await Database.Subcategories.GetAll();
            return _mapper.Map<IEnumerable<SubcategoryDTO>>(subcategories);
        }

        public async Task<SubcategoryDTO> Get(int id)
        {
            var subcategory = await Database.Subcategories.Get(id);
            if(subcategory == null)
                throw new ValidationExceptionFromService("There is no subcategory with this id", "");
            return new SubcategoryDTO
            {
                Id = subcategory.Id,
                Name = subcategory.Name
            };
        }

        public async Task Create(SubcategoryDTO subcategoryDTO)
        {
            var subcategory = new Subcategory
            {
                Name = subcategoryDTO.Name
            };
            await Database.Subcategories.Create(subcategory);
            await Database.Save();

        }

        public async Task Update(SubcategoryDTO subcategoryDTO)
        {
            var subcategory = new Subcategory
            {
                Id = subcategoryDTO.Id,
                Name = subcategoryDTO.Name
            };
            Database.Subcategories.Update(subcategory);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Subcategories.Delete(id);
            await Database.Save();
        }
    }
}
