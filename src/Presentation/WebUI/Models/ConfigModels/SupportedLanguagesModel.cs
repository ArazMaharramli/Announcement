using System.Collections.Generic;

namespace WebUI.Models.ConfigModels
{
    public class SupportedLanguages
    {
        public List<LanguageModel> Languages { get; set; }
    }
    public class LanguageModel
    {
        public string DisplayName { get; set; }
        public string Culture { get; set; }
        public string Flag { get; set; }
        public bool IsDefault { get; set; }
    }
}

