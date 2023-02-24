namespace Application.Common.Models.ConfigModels;

public class LanguageModel
{
    public string DisplayName { get; set; }
    public string CountryCode { get; set; }
    public string Culture { get; set; }
    public string Flag { get; set; }
    public bool IsDefault { get; set; }
}