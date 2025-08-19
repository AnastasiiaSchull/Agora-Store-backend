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
    public class CountryService : ICountryService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public CountryService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CountryDTO>> GetAll()
        {
            var countries = await Database.Countries.GetAll();
            return _mapper.Map<IQueryable<Country>, IEnumerable<CountryDTO>>(countries);
        }

        public async Task<CountryDTO> Get(int id)
        {
            var country = await Database.Countries.Get(id);
            if (country == null)
                throw new ValidationExceptionFromService("There is no country with this id", "");
            return new CountryDTO
            {
                Id = country.Id,
                Name = country.Name,
            };
        }

        public async Task Create(CountryDTO countryDTO)
        {
            var country = new Country
            {
                Id = countryDTO.Id,
                Name = countryDTO.Name,
            };
            await Database.Countries.Create(country);
            await Database.Save();
        }
        public async Task Update(CountryDTO countryDTO)
        {
            var country = new Country
            {
                Id = countryDTO.Id,
                Name = countryDTO.Name,
            };
            Database.Countries.Update(country);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Countries.Delete(id);
            await Database.Save();
        }
    }
}
