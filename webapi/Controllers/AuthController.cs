using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pinsetur.Webapi.Dto;
using Pinsetur.Webapi.Services;

namespace Pinsetur.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IRefreshTokenService _refreshTokenService;
        private const string CookieName = "refreshToken";

        public AuthController(ILoginService loginService, IRefreshTokenService refreshTokenService)
        {
            _loginService = loginService;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Login([FromForm] LoginDto login)
        {
            SessionDto? session = _loginService.Login(login);

            if (session is null)
                return StatusCode(StatusCodes.Status400BadRequest);

            string refreshToken = _refreshTokenService.CreateRefreshToken(session.User.UserName);
            SetRefreshTokenCookie(refreshToken);

            return Ok(session);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Refresh()
        {
            var token = Request.Cookies[CookieName];
            if (token is null)
                return Unauthorized();

            var result = _refreshTokenService.Refresh(token);
            if (result is null)
                return Unauthorized();

            SetRefreshTokenCookie(result.Value.newToken);
            return Ok(result.Value.session);
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            var token = Request.Cookies[CookieName];
            if (token is not null)
                _refreshTokenService.Delete(token);

            Response.Cookies.Delete(CookieName);
            return Ok();
        }

        private void SetRefreshTokenCookie(string token)
        {
            Response.Cookies.Append(CookieName, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });
        }
    }
}
