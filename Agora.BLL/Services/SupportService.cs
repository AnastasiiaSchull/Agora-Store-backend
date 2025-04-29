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
using Microsoft.EntityFrameworkCore;

namespace Agora.BLL.Services
{
    public class SupportService : ISupportService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public SupportService(IUnitOfWork database, IMapper mapper)
        {
            Database = database;
            _mapper = mapper;
        }

        public async Task<IQueryable<SupportDTO>> GetAll()
        {
            var supports = await Database.Supports.GetAll();
            return _mapper.Map<IQueryable<SupportDTO>>(supports.ToList());
        }
        public async Task<SupportDTO> Get(int id)
        {
            var support = await Database.Supports.Get(id);
            if(support == null)
                throw new ValidationExceptionFromService("There is no row with this id", "");
            return new SupportDTO
            {
                Id = support.Id,
                Subject = support.Subject,
                Message = support.Message,
                Status = support.Status,
                Date = support.Date

            };
        }
        public async Task Create(SupportDTO supportDTO)
        {
            var support = new Support
            {
                Subject = supportDTO.Subject,
                Message = supportDTO.Message,
                Status = supportDTO.Status,
                Date = supportDTO.Date

            };
            await Database.Supports.Create(support);
            await Database.Save();
        }
        public async Task Update(SupportDTO supportDTO)
        {
            var support = new Support
            {
                Id = supportDTO.Id,
                Subject = supportDTO.Subject,
                Message = supportDTO.Message,
                Status = supportDTO.Status,
                Date = supportDTO.Date

            };
             Database.Supports.Update(support);
            await Database.Save();
        }
        public async Task Delete(int id)
        {
            await Database.Supports.Delete(id);
            await Database.Save();
        }
    }
}
