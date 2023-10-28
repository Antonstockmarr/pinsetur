using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using stockmarrdk_api.Dto;
using stockmarrdk_api.Models;
using stockmarrdk_api.Services;

namespace stockmarrdk_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Login([FromForm] LoginDto login)
        {
            SessionDto? user = _loginService.Login(login);

            if (user is null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, user);
            }
        }
    }
}
