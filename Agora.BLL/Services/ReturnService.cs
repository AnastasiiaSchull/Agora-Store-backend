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
    public class ReturnService : IReturnService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public ReturnService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<ReturnDTO>> GetAll()
        {
            var returns = await Database.Returns.GetAll();
            return _mapper.Map<IQueryable<ReturnDTO>>(returns.ToList());

        }
        public async Task<ReturnDTO> Get(int id)
        {
            var oneReturn = await Database.Returns.Get(id);
            if (oneReturn == null)
                throw new ValidationExceptionFromService("There is no return with this id", "");
            return new ReturnDTO
            {
                Id = oneReturn.Id,
                ReturnDate = oneReturn.ReturnDate,
                Status = oneReturn.Status,
                RefundAmount = oneReturn.RefundAmount

            };
        }

        public async Task Create(ReturnDTO returnDTO)
        {
            var oneReturn = new Return
            {
                ReturnDate = returnDTO.ReturnDate,
                Status = returnDTO.Status,
                RefundAmount = returnDTO.RefundAmount

            };
            await Database.Returns.Create(oneReturn);
            await Database.Save();
        }
        public async Task Update(ReturnDTO returnDTO)
        {
            var oneReturn = new Return
            {
                Id = returnDTO.Id,
                ReturnDate = returnDTO.ReturnDate,
                Status = returnDTO.Status,
                RefundAmount = returnDTO.RefundAmount

            };
            Database.Returns.Update(oneReturn);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Returns.Delete(id);
            await Database.Save();
        }
    }
}
