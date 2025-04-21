using Pinsetur.Webapi.Dto;
using Pinsetur.Webapi.Models;

namespace Pinsetur.Webapi.Services
{
    public interface ILoginService
    {
        SessionDto? Login(LoginDto login);
    }
}
