using DemoBlazor.Server.Services.Authorize;
using DemoBlazor.Shared.Authorize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoBlazor.Server.Controllers.Authorize
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;

        public AuthorizeController(IJwtTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("Login")]
        public IActionResult Login([FromBody] TokenViewModel model)
        {
            if (model != null && !string.IsNullOrEmpty(model.Email))
            {
                var token = _tokenService.BuildToken(model.Email);
                model.Password = null;
                model.Token = token;
                return Ok(model);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}