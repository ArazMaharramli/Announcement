using System;
using System.Collections.Generic;

namespace WebUI.Areas.Admin.ViewModels.Translations
{
    public class TranslationViewModel
    {
        public string Key { get; set; }
        public string Text { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<TranslationDto> Translations { get; set; }

    }
    public class TranslationDto
    {
        public long Id { get; set; }
        public string LangCode { get; set; }
        public string Text { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
