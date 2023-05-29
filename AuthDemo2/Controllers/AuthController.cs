using AuthDemo2.Models;
using AuthDemo2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<bool> RegisterUser(LoggedUser user)
        {
            return await _authService.Register(user);            
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoggedUser user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await _authService.Login(user))
            {
                var tokenString = _authService.GenerateToken(user);

                return Ok(tokenString);             
            }
            return BadRequest();
        }
    }
}
