using Microsoft.AspNetCore.Mvc;
using stockmarrdk_api.Dto;
using stockmarrdk_api.Services;

namespace stockmarrdk_api.Controllers
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public ActionResult GetToken()
        {
            string token = _tokenService.GetToken();
            return StatusCode(StatusCodes.Status200OK, token);
        }
    }
}