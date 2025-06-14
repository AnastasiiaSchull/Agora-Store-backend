using Agora.BLL.Interfaces;
using Agora.Models;
using Microsoft.AspNetCore.Mvc;


namespace Agora.Controllers
{
    [ApiController]
    [Route("api/translate")]
    public class LanguageController : ControllerBase
    {
        private readonly ITranslationService _translationService;

        public LanguageController(ITranslationService translationService)
        {
            _translationService = translationService;
        }


        [HttpPost]
        public async Task<IActionResult> Translate([FromBody] TranslateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
                return BadRequest("Text is required");

            var translated = await _translationService.Translate(request.Text, request.Locale);
            return Ok(new { translatedText = translated });
        }
    }
}
