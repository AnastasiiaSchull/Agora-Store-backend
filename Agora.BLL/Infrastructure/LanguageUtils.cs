namespace Agora.BLL.Infrastructure
{
    public static class LanguageUtils
    {
        public static string NormalizeSpeechLocale(string? locale)
        {
            return locale?.ToLower() switch
            {
                "ua" => "uk-UA",//locale = "uk"  стандартная поддержка
                "uk" => "uk-UA",
                "ru" => "ru-RU",
                "en" => "en-US",
                "de" => "de-DE",
                "fr" => "fr-FR",
                _ => "en-US"    //fallback на английский
            };
        }

        public static string NormalizeTranslationLanguage(string? locale)
        {
            return locale?.ToLower() switch
            {
                "ua" => "uk",
                "uk" => "uk",
                "ru" => "ru",
                "de" => "de",
                "fr" => "fr",
                _ => "en"
            };
        }
    }
}
