using Pinsetur.Webapi.Models;

namespace Pinsetur.Webapi.Services
{
    public interface IHomepageConfigService
    {
        HomepageConfig Get();
        HomepageConfig Save(HomepageConfig config);
    }
}
