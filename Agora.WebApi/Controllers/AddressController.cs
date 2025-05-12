using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        // GET: api/addresses/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var address = await _addressService.Get(id);
            return Ok(address);
        }

        // GET: api/addresses/user/id
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var addresses = await _addressService.GetByUserId(userId);
            return Ok(addresses);
        }

    }
}
