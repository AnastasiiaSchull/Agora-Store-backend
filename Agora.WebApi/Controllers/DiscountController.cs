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
        private readonly IProductService _productService;
        public DiscountController(IDiscountService discountService, IProductService productService)
        {
            _discountService = discountService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var discounts = await _discountService.GetAll();
            return Ok(discounts);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var activeDiscounts = await _discountService.GetActiveDiscounts();
            return Ok(activeDiscounts);
        }


        [HttpPost("apply-discounts")]
        public async Task<IActionResult> ApplyDiscounts()
        {
            await _productService.UpdateAllDiscountedPrices();
            return Ok("Discounts applied to all products");
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
            await _productService.UpdateAllDiscountedPrices();

            return Ok("Discount created and applied to products");          
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DiscountDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _discountService.Update(dto);
            await _productService.UpdateAllDiscountedPrices();
            return Ok("Discount updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _discountService.Delete(id);
            await _productService.UpdateAllDiscountedPrices();
            return Ok();
        }
    }

}
