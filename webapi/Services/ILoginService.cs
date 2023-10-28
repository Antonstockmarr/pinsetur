using stockmarrdk_api.Dto;
using stockmarrdk_api.Models;

namespace stockmarrdk_api.Services
{
    public interface ILoginService
    {
        SessionDto? Login(LoginDto login);
    }
}
