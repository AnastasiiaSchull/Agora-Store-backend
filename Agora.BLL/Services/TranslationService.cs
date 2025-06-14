using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Agora.BLL.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly IConfiguration _config;

        public TranslationService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> Translate(string input, string fromLocale)
        {
            string key = _config["AZURE_TRANSLATOR_KEY"]!;
            string endpoint = _config["AZURE_TRANSLATOR_ENDPOINT"]!;
            string region = _config["AZURE_SPEECH_REGION"]!;

            string fromLang = LanguageUtils.NormalizeTranslationLanguage(fromLocale);       

            string route = $"/translate?api-version=3.0&from={fromLang}&to=en";

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