namespace Application.Common.Interfaces
{
    public interface ICurrentLanguageService
    {
        string LangCode { get; }
        string DisplayName { get; }
        bool IsDefault { get; }
    }
}
