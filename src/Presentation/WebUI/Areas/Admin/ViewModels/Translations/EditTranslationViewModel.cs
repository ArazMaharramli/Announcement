namespace WebUI.Areas.Admin.ViewModels.Translations
{
    public class EditTranslationViewModel
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string LangCode { get; set; }
        public string Text { get; set; }
        public string ResourceKey { get; set; }
    }
}
