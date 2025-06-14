using Agora.BLL.DTO;
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


        [HttpPost("create-address")]
        public async Task<IActionResult> Create([FromBody] AddressDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _addressService.CreateAddress(dto);
            return Ok(new { message = "Address created successfully" });
        }

        [HttpPut("update-address/{id}")]
        public async Task<IActionResult> Update([FromBody] AddressDTO dto, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _addressService.UpdateAddress(dto, id);
            return Ok(new { message = "Address updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _addressService.DeleteAddress(id);
            return Ok(new { message = "Address deleted successfully" });
        }
    }
}
