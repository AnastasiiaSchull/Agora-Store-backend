using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/subcategories")]
    public class SubcategoryController : ControllerBase
    {
        private readonly ISubcategoryService _subcategoryService;

        public SubcategoryController(ISubcategoryService subcategoryService)
        {
            _subcategoryService = subcategoryService;
        }

        // GET: api/subcategories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subcategories = await _subcategoryService.GetAll();
            return Ok(subcategories);
        }

        // GET: api/subcategories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var subcategory = await _subcategoryService.Get(id);
            return Ok(subcategory);
        }

        // POST: api/subcategories
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SubcategoryDTO subcategory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CategoryDTO category = new CategoryDTO
            {
                Id = subcategory.CategoryId
            };
            subcategory.CategoryDTO = category; 
            await _subcategoryService.Create(subcategory);
            return Ok("Subcategory created");
        }

        // PUT: api/subcategories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] SubcategoryDTO subcategory)
        {
            if (id != subcategory.Id)
                return BadRequest("Subcategory ID mismatch");

            CategoryDTO category = new CategoryDTO
            {
                Id = subcategory.CategoryId
            };
            subcategory.CategoryDTO = category;
            await _subcategoryService.Update(subcategory);
            return Ok("Subcategory updated");
        }

        // DELETE: api/subcategories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _subcategoryService.Delete(id);
            return Ok("Subcategory deleted");
        }
    }
}
