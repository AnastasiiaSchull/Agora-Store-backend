using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/brands")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        // GET: api/brands
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandService.GetAll();
            return Ok(brands);
        }

        // GET: api/brands/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var brand = await _brandService.Get(id);
            return Ok(brand);
        }

        [HttpGet("by-subcategory")]
        public async Task<IActionResult> GetBrandsBySubcategory([FromQuery] int subcategoryId)
        {
            var brands = await _brandService.GetBySubcategory(subcategoryId);
            return Ok(brands);
        }

        // POST: api/brands
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BrandDTO brand)
        {
            if (string.IsNullOrWhiteSpace(brand.Name))
                return BadRequest("Brand name is required.");

            await _brandService.Create(brand);
            return Ok("Brand created");
        }

        // PUT: api/brands/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BrandDTO brand)
        {
            if (id != brand.Id)
                return BadRequest("Brand ID mismatch");

            await _brandService.Update(brand);
            return Ok("Brand updated");
        }

        // DELETE: api/brands/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _brandService.Delete(id);
            return Ok("Brand deleted");
        }
    }
}
