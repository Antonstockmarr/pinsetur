using Pinsetur.Webapi.Models;
using Pinsetur.Webapi.Repository;

namespace Pinsetur.Webapi.Services
{
    public class HomepageConfigService : IHomepageConfigService
    {
        private readonly IHomepageConfigRepository _repository;

        public HomepageConfigService(IHomepageConfigRepository repository)
        {
            _repository = repository;
        }

        public HomepageConfig Get()
        {
            var config = _repository.Get();

            if (string.IsNullOrEmpty(config.WelcomeTitle))
                config.WelcomeTitle = "Velkommen til Pinsetur.dk";
            if (string.IsNullOrEmpty(config.WelcomeText))
                config.WelcomeText = "Hvert år mødes familien til pinsetur.";
            if (string.IsNullOrEmpty(config.UploadPrompt))
                config.UploadPrompt = "Har du billeder fra turen? Del dem med familien.";

            return config;
        }

        public HomepageConfig Save(HomepageConfig config)
        {
            _repository.Save(config);
            return config;
        }
    }
}
