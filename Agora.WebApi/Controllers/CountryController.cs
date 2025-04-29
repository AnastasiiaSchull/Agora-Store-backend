using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Agora.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        

        public CountryController(ICountryService countryService)

        {
            _countryService = countryService;            
        }
        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countryService.GetAll();
            return Ok(countries);
        }
    }
}
