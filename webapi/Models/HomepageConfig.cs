namespace Pinsetur.Webapi.Models
{
    public class HomepageConfig
    {
        public string WelcomeTitle { get; set; } = "";
        public string WelcomeText { get; set; } = "";
        public string UploadPrompt { get; set; } = "";
        public List<int> FrontpageImageIds { get; set; } = new();
    }
}
