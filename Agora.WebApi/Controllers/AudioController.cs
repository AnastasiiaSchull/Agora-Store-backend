using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Newtonsoft.Json;
using System.Text;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/audio")]
    public class AudioController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AudioController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAudio([FromForm] IFormFile audio)
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
            config.SpeechRecognitionLanguage = "en-US";

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

            // переводим текст, если надо
            var translated = await TranslateText(recognizedText);

            return Ok(new
            {
                Text = recognizedText,
                TranslatedText = translated
            });
        }

        private async Task<string> TranslateText(string input)
        {
            string key = _config["AZURE_TRANSLATOR_KEY"]!;
            string endpoint = _config["AZURE_TRANSLATOR_ENDPOINT"]!;
            string region = _config["AZURE_SPEECH_REGION"]!;

            string route = "/translate?api-version=3.0&from=en&to=en";

            object[] body = new object[] { new { Text = input } };
            var bodyJson = JsonConvert.SerializeObject(body);

            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint + route)
            {
                Content = new StringContent(bodyJson, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            request.Headers.Add("Ocp-Apim-Subscription-Region", region);

            var response = await client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(json)!;
            return result[0].translations[0].text;
        }
    }
}
