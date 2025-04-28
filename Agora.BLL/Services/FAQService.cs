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
    public class FAQService : IFAQService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public FAQService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<FAQDTO>> GetAll()
        {
            var faqs = await Database.FAQs.GetAll();
            return _mapper.Map<IQueryable<FAQDTO>>(faqs.ToList());
        }

        public async Task<FAQDTO> Get(int id)
        {
            var faq = await Database.FAQs.Get(id);
            if (faq == null)
                throw new ValidationExceptionFromService("There is no FAQ with this id", "");
            return new FAQDTO
            {
                Id = faq.Id,
                Question = faq.Question,
                Answer = faq.Answer,
                FAQCategoryId = faq.FAQCategory.Id,
            };
        }

        public async Task Create(FAQDTO faqDTO)
        {
            var faq = new FAQ
            {
                Id = faqDTO.Id,
                Question = faqDTO.Question,
                Answer = faqDTO.Answer,
            };
            await Database.FAQs.Create(faq);
            await Database.Save();
        }
        public async Task Update(FAQDTO faqDTO)
        {
            var faq = new FAQ
            {
                Id = faqDTO.Id,
                Question = faqDTO.Question,
                Answer = faqDTO.Answer,
            };
            Database.FAQs.Update(faq);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.FAQs.Delete(id);
            await Database.Save();
        }
    }
}
