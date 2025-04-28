using Agora.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Interfaces
{
    public interface ICountryService
    {
        Task<IEnumerable<CountryDTO>> GetAll();
        Task<CountryDTO> Get(int id);
        Task Create(CountryDTO countryDTO);
        Task Update(CountryDTO countryDTO);
        Task Delete(int id);
    }
}
