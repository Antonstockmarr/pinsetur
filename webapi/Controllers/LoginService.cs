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

        public LoginController()
        {
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        public ActionResult Login()
        {
            var user = new UserDto { Jwt = "ABCDEFG", Username = "John Doe"};

            return StatusCode(StatusCodes.Status200OK, user);
        }

    }
}
