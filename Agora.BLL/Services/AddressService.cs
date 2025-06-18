using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Agora.DAL.Repository;
using AutoMapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                CountryId = address.CountryId
            };
        }

        public async Task<AddressDTO> GetByUserIdAsync(int userId)
        {
            var address = await Database.Addresses.GetAddressByUserIdAsync(userId);
            if (address == null)
                return null;

            return new AddressDTO
            {
                Id = address.Id,
                Building = address.Building,
                Appartement = address.Appartement,
                Street = address.Street,
                City = address.City,
                PostalCode = address.PostalCode,
                Country = address.Country.Name,
            };
        }

        public async Task<IEnumerable<AddressDTO>> GetByUserId(int userId)
        {
            var user = await Database.Users.Get(userId);

            if (user == null)
                throw new ValidationExceptionFromService("User not found", "");

            var addresses = await Database.Addresses.GetWithCountryByUserId(userId);

            if (!addresses.Any())
                return Enumerable.Empty<AddressDTO>();

            return _mapper.Map<IEnumerable<AddressDTO>>(addresses);
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

        public async Task UpdateSellerAddressAsync(UpdateSellerAddressDTO dto)
        {
            var address = await Database.Addresses.Get(dto.AddressId);

            if (address == null)
                throw new ValidationExceptionFromService("Address not found", "");

            address.Building = dto.Building;
            address.Appartement = dto.Appartement;
            address.Street = dto.Street;
            address.City = dto.City;
            address.PostalCode = dto.PostalCode;
            address.CountryId = dto.CountryId;

            Database.Addresses.Update(address);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Addresses.Delete(id);
            await Database.Save();
        }

        public async Task CreateAddress(AddressDTO dto)
        {
            var user = await Database.Users.Get(dto.UserId);
            if (user == null)
                throw new ValidationExceptionFromService("User not found", "");

            var address = new Address
            {
                Building = dto.Building,
                Appartement = dto.Appartement,
                Street = dto.Street,
                City = dto.City,
                PostalCode = dto.PostalCode,
                CountryId = dto.CountryId
            };

            await Database.Addresses.Create(address);
            await Database.Save();

            var addressUser = new AddressUser
            {
                AddressesId = address.Id,
                UserId = user.Id
            };

            await Database.AddressUser.Create(addressUser);
            await Database.Save();
        }

        public async Task UpdateAddress(AddressDTO dto, int id)
        {
            var existingAddress = await Database.Addresses.Get(id);
            if (existingAddress == null)
                throw new Exception($"Address with ID {id} not found.");

            existingAddress.CountryId = dto.CountryId;
            existingAddress.City = dto.City;
            existingAddress.PostalCode = dto.PostalCode;
            existingAddress.Street = dto.Street;
            existingAddress.Building = dto.Building;
            existingAddress.Appartement = dto.Appartement;

            Database.Addresses.Update(existingAddress);
            await Database.Save();
        }

        public async Task DeleteAddress(int id)
        {
            var address = await Database.Addresses.Get(id);
            if (address == null)
                throw new Exception($"Address with ID {id} not found.");

            await Database.AddressUser.Delete(id);
            await Database.Addresses.Delete(id);
            await Database.Save();
        }
    }
}
