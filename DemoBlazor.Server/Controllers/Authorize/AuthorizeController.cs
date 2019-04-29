using DemoBlazor.Server.Services.Authorize;
using DemoBlazor.Shared.Authorize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DemoBlazor.Server.Controllers.Authorize
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;
        private UserManager<IdentityUser> _userManager;
        public AuthorizeController(IJwtTokenService tokenService, UserManager<IdentityUser> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration([FromBody] TokenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _userManager.CreateAsync(new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            }, model.Password);

            if (!result.Succeeded)
            {
                return StatusCode(500);
            } else
            {
                model.Password = null;
                model.Token = _tokenService.BuildToken(model.Email);
                return Ok(model);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] TokenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            var correctUser = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!correctUser)
            {
                return BadRequest("Username or password is incorrect!");
            } else
            {
                model.Password = null;
                model.Token = _tokenService.BuildToken(model.Email);
                return Ok(model);
            }
        }
    }
}