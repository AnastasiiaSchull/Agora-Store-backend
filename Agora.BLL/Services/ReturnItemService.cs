using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using AutoMapper;
using Agora.BLL.Infrastructure;

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

        public async Task<List<ReturnItemDTO>> GetNewReturns(int storeId)
        {
            var returnItems = await Database.ReturnItems.GetNewReturns(storeId);
            return _mapper.Map<List<ReturnItemDTO>>(returnItems.ToList());

        }

        public async Task<List<ReturnItemDTO>> GetAllByStore(int storeId)
        {
            var returnItem = await Database.ReturnItems.GetAllByStore(storeId);
            return _mapper.Map<List<ReturnItemDTO>>(returnItem.ToList());

        }

        public async Task<List<ReturnItemDTO>> GetFiltredReturns(int storeId, string field, string value)
        {

            var returnItem = await Database.ReturnItems.GetFilteredReturns(storeId, field, value);
            return _mapper.Map<List<ReturnItemDTO>>(returnItem.ToList());

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

        public async Task<ReturnItemDetailsDTO> GetReturnItemDetails(int id)
        {
            var returnItem = await Database.ReturnItems.Get(id);
            if (returnItem == null)
                throw new ValidationExceptionFromService("There is no return item with this id", "");
            
            return new ReturnItemDetailsDTO
            {
                ReturnItemId = returnItem.Id,
                ReturnDate = returnItem.Return.ReturnDate,
                Reason = returnItem.Reason,
                Status = returnItem.Return.Status.ToString(),

                ProductImage = returnItem.Product?.ImagesPath,
                ProductName = returnItem.Product?.Name,

                OrderItemId = returnItem.OrderItemId,
                OrderDate = returnItem.OrderItem?.Date,
                Price = returnItem.OrderItem?.PriceAtMoment,

                UserName = returnItem.Return.Customer.User?.Name,
                UserSurname = returnItem.Return.Customer.User?.Surname,

                Building = returnItem.OrderItem.Shipping.Address.Building,
                Appartement = returnItem.OrderItem.Shipping.Address.Appartement,
                Street = returnItem.OrderItem.Shipping.Address.Street,
                City = returnItem.OrderItem.Shipping.Address.City,
                Country = returnItem.OrderItem.Shipping.Address.Country.Name,

                Telephone = returnItem.Return.Customer.User?.PhoneNumber,
                Email = returnItem.Return.Customer.User?.Email
            };
        }

        public async Task<int> Create(ReturnItemDTO returnItemDTO)
        {
            var returnItem = new ReturnItem
            {
                Id = returnItemDTO.Id,
                Quantity = returnItemDTO.Quantity,
                Reason = returnItemDTO.Reason,
                ReturnId = returnItemDTO.ReturnId,
                ProductId = returnItemDTO.ProductId,
                OrderItemId = returnItemDTO.OrderItemId
            };
            await Database.ReturnItems.Create(returnItem);
            await Database.Save();

            return returnItem.Id;
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
