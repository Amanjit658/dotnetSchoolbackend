using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myFirstSchoolProject.DTOs.Auth;
using myFirstSchoolProject.Models;
using myFirstSchoolProject.Services;
using myFirstSchoolProject.Services.Implementations;

namespace myFirstSchoolProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return Unauthorized("Invalid email");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
                return Unauthorized("Invalid password");

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _tokenService.CreateTokenAsync(user);

            return Ok(new
            {
                token,
                roles
            });
        }
    }
}
