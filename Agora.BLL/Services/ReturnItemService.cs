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
    public class ReturnItemService : IReturnItemService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public ReturnItemService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<ReturnItemDTO>> GetAll()
        {
            var returnItem = await Database.ReturnItems.GetAll();
            return _mapper.Map<IQueryable<ReturnItemDTO>>(returnItem.ToList());
        }

        public async Task<ReturnItemDTO> Get(int id)
        {
            var returnItem = await Database.ReturnItems.Get(id);
            if (returnItem == null)
                throw new ValidationExceptionFromService("There is no return item with this id", "");
            return new ReturnItemDTO
            {
                Id = returnItem.Id,
                Quantity = returnItem.Quantity,
                Reason = returnItem.Reason,
                ReturnId = returnItem.Return.Id,
                ProductId = returnItem.Product.Id,
            };
        }

        public async Task Create(ReturnItemDTO returnItemDTO)
        {
            var returnItem = new ReturnItem
            {
                Id = returnItemDTO.Id,
                Quantity = returnItemDTO.Quantity,
                Reason = returnItemDTO.Reason,
            };
            await Database.ReturnItems.Create(returnItem);
            await Database.Save();
        }
        public async Task Update(ReturnItemDTO returnItemDTO)
        {
            var returnItem = new ReturnItem
            {
                Id = returnItemDTO.Id,
                Quantity = returnItemDTO.Quantity,
                Reason = returnItemDTO.Reason,
            };
            Database.ReturnItems.Update(returnItem);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.ReturnItems.Delete(id);
            await Database.Save();
        }
    }
}
