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
    public class FAQCategoryService : IFAQCategoryService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public FAQCategoryService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<FAQCategoryDTO>> GetAll()
        {
            var faqCategory = await Database.FAQCategories.GetAll();
            return _mapper.Map<IQueryable<FAQCategoryDTO>>(faqCategory.ToList());
        }

        public async Task<FAQCategoryDTO> Get(int id)
        {
            var faqCategory = await Database.FAQCategories.Get(id);
            if (faqCategory == null)
                throw new ValidationExceptionFromService("There is no FAQ category with this id", "");
            return new FAQCategoryDTO
            {
                Id = faqCategory.Id,
                Name = faqCategory.Name,
            };
        }

        public async Task Create(FAQCategoryDTO faqCategoryDTO)
        {
            var faqCategory = new FAQCategory
            {
                Id = faqCategoryDTO.Id,
                Name = faqCategoryDTO.Name,
            };
            await Database.FAQCategories.Create(faqCategory);
            await Database.Save();
        }
        public async Task Update(FAQCategoryDTO faqCategoryDTO)
        {
            var faqCategory = new FAQCategory
            {
                Id = faqCategoryDTO.Id,
                Name = faqCategoryDTO.Name,
            };
            Database.FAQCategories.Update(faqCategory);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.FAQCategories.Delete(id);
            await Database.Save();
        }
    }
}
