using System.Security.Claims;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        readonly UserManager<ApplicationUser> userManager;
        readonly SignInManager<ApplicationUser> signInManager;
        readonly IJwtTokenGenerator jwtTokenGenerator;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {
            var result = await userManager.CreateAsync(new ApplicationUser() { Email = request.email, UserName = request.username }, request.password);
            if (result.Succeeded)
                return Ok();
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await userManager.FindByEmailAsync(email: request.email);
            if (user != null)
            {
                var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.password, false);
                if (signInResult.Succeeded)
                {
                    var token = jwtTokenGenerator.GenerateToken(Guid.Parse(user.Id), request.email);
                    return Ok(new { token });
                }
                else
                    return Unauthorized();
            }
            else
                return NotFound();  
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var name = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(new { name, email });
        }

        public record CreateUserRequest(string username, string email, string password);
        public record LoginRequest(string email, string password);
    }
}