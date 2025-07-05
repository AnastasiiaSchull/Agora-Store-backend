using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var discounts = await _discountService.GetAll();
            return Ok(discounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var discount = await _discountService.Get(id);
            if (discount == null)
                return NotFound();

            return Ok(discount);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DiscountDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _discountService.Create(dto);
            return Ok("Discount created");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DiscountDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _discountService.Update(dto);
            return Ok("Discount updated");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _discountService.Delete(id);
            return Ok();
        }
    }

}
