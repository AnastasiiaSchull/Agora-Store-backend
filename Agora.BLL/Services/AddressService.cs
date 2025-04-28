using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.DAL.Interfaces;
using AutoMapper;
using Agora.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.BLL.Services
{
    public class AddressService : IAddressService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public AddressService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<AddressDTO>> GetAll()
        {
            var addresses = await Database.Addresses.GetAll();
            return _mapper.Map<IQueryable<AddressDTO>>(addresses.ToList());
        }

        public async Task<AddressDTO> Get(int id)
        {
            var address = await Database.Addresses.Get(id);
            if (address == null)
                throw new ValidationExceptionFromService("There is no address with this id", "");
            return new AddressDTO
            {
                Id = address.Id,
                Building = address.Building,
                Appartement = address.Appartement,
                Street = address.Street,
                City = address.City,
                PostalCode = address.PostalCode,
                CountryId = address.Country.Id
            };
        }

        public async Task Create(AddressDTO addressDTO)
        {
            var address = new Address
            {
                Id = addressDTO.Id,
                Building = addressDTO.Building,
                Appartement = addressDTO.Appartement,
                Street = addressDTO.Street,
                City = addressDTO.City,
                PostalCode = addressDTO.PostalCode,
                CountryId = addressDTO.CountryId
            };
            await Database.Addresses.Create(address);
            await Database.Save();
        }
        public async Task Update(AddressDTO addressDTO)
        {
            var address = new Address
            {
                Id = addressDTO.Id,
                Building = addressDTO.Building,
                Appartement = addressDTO.Appartement,
                Street = addressDTO.Street,
                City = addressDTO.City,
                PostalCode = addressDTO.PostalCode,
                CountryId = addressDTO.CountryId
            };
            Database.Addresses.Update(address);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Addresses.Delete(id);
            await Database.Save();
        }
    }
}
