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

        [HttpGet("seller-address/{sellerId}")]
        public async Task<IActionResult> GetSellerAddress(int sellerId)
        {
            try
            {
                var address = await _addressService.GetByUserIdAsync(sellerId);
                if (address == null)
                    return NotFound(new { message = "Address not found" });

                return Ok(new
                {
                    country = address.Country,
                    city = address.City,
                    street = address.Street,
                    building = address.Building,
                    appartement = address.Appartement,
                    postalСode = address.PostalCode,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
