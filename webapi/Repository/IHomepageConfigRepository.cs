using Pinsetur.Webapi.Models;

namespace Pinsetur.Webapi.Repository
{
    public interface IHomepageConfigRepository
    {
        HomepageConfig Get();
        void Save(HomepageConfig config);
    }
}
