namespace Agora.BLL.Interfaces
{
    public interface ITranslationService
    {
        Task<string> Translate(string input, string fromLocale);
    }
}
