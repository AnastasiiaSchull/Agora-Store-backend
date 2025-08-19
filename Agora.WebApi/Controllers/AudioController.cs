using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/audio")]
    public class AudioController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITranslationService _translationService;
        public AudioController(IConfiguration config, ITranslationService translationService)
        {
            _config = config;
            _translationService = translationService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAudio([FromForm] IFormFile audio, [FromForm] string locale)
        {
            if (audio == null || audio.Length == 0)
                return BadRequest("No audio uploaded");

            var tempPath = Path.GetTempFileName();
            using (var fs = new FileStream(tempPath, FileMode.Create))
            {
                await audio.CopyToAsync(fs);
            }

            // ключи из конфигурации 
            string speechKey = _config["AZURE_SPEECH_KEY"]!;
            string speechRegion = _config["AZURE_SPEECH_REGION"]!;

            var config = SpeechConfig.FromSubscription(speechKey, speechRegion);

            config.SpeechRecognitionLanguage = LanguageUtils.NormalizeSpeechLocale(locale);

            // pаспознавание
            string recognizedText = "";
            using (var audioInput = AudioConfig.FromWavFileInput(tempPath))
            using (var recognizer = new SpeechRecognizer(config, audioInput))
            {
                var result = await recognizer.RecognizeOnceAsync();
                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    recognizedText = result.Text;
                }
                else
                {
                    return BadRequest("Speech not recognized.");
                }
            }

            System.IO.File.Delete(tempPath);

            // используем сервис перевода
            string translated = locale == "en"
                ? recognizedText
                : await _translationService.Translate(recognizedText, locale);

            return Ok(new
            {
                Text = recognizedText,
                TranslatedText = translated
            });
        }

    }
}
