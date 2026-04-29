using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pinsetur.Webapi.Models;
using Pinsetur.Webapi.Services;

namespace Pinsetur.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomepageConfigController : ControllerBase
    {
        private readonly IHomepageConfigService _service;

        public HomepageConfigController(IHomepageConfigService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize("Users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HomepageConfig))]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpPatch]
        [Authorize("Admins")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HomepageConfig))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult Patch([FromBody] HomepageConfig config)
        {
            try
            {
                return Ok(_service.Save(config));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
