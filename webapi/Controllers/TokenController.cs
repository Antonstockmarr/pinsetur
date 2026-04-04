using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pinsetur.Webapi.Dto;
using Pinsetur.Webapi.Services;

namespace Pinsetur.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet()]
        [Authorize("Users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public ActionResult GetToken()
        {
            string token = _tokenService.GetToken();
            return StatusCode(StatusCodes.Status200OK, token);
        }
    }
}