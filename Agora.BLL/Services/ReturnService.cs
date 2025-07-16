using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Agora.Enums;
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
                Status = oneReturn.Status.ToString(),
                RefundAmount = oneReturn.RefundAmount
            };
        }

        public async Task<int> Create(ReturnDTO returnDTO)
        {
            var Return = new Return
            {
                ReturnDate = returnDTO.ReturnDate,                
                Status = Enum.Parse<ReturnStatus>(returnDTO.Status, ignoreCase: true),
                RefundAmount = returnDTO.RefundAmount,
                OrderId = returnDTO.OrderId,
                CustomerId = returnDTO.CustomerId
            };

            await Database.Returns.Create(Return);
            await Database.Save();

            return Return.Id;
        }
        public async Task Update(ReturnDTO returnDTO)
        {
            var oneReturn = new Return
            {
                Id = returnDTO.Id,
                ReturnDate = returnDTO.ReturnDate,                
                Status = Enum.Parse<ReturnStatus>(returnDTO.Status, true),
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
