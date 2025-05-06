using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FAQController : ControllerBase
    {
        private readonly IFAQService _faqService;
        private readonly IFAQCategoryService _categoryService;

        public FAQController(IFAQService faqService, IFAQCategoryService categoryService)
        {
            _faqService = faqService;
            _categoryService = categoryService;
        }

        //FAQ 

        [HttpGet("faqs")]
        public async Task<IActionResult> GetAllFAQs()
        {
            var faqs = await _faqService.GetAll();
            return Ok(faqs);
        }

        [HttpGet("faqs/{id}")]
        public async Task<IActionResult> GetFAQ(int id)
        {
            var faq = await _faqService.Get(id);
            return Ok(faq);
        }

        [HttpPost("faqs")]
        public async Task<IActionResult> CreateFAQ([FromBody] FAQDTO dto)
        {
            await _faqService.Create(dto);
            return Ok();
        }

        [HttpPut("faqs")]
        public async Task<IActionResult> UpdateFAQ([FromBody] FAQDTO dto)
        {
            await _faqService.Update(dto);
            return Ok();
        }

        [HttpDelete("faqs/{id}")]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            await _faqService.Delete(id);
            return Ok();
        }

        //Categories

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAll();
            return Ok(categories);
        }

        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryService.Get(id);
            return Ok(category);
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] FAQCategoryDTO dto)
        {
            await _categoryService.Create(dto);
            return Ok();
        }

        [HttpPut("categories")]
        public async Task<IActionResult> UpdateCategory([FromBody] FAQCategoryDTO dto)
        {
            await _categoryService.Update(dto);
            return Ok();
        }

        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.Delete(id);
            return Ok("The category has been deleted. All related questions were also removed.");
        }
    }
}
