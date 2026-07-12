using BlazorApp1.Entities;
using BlazorApp1.Services.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.Controllers
{
    [ApiController]
    [Route("")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromForm] LoginRequest request, [FromQuery] string? returnUrl)
        {
            var result = await authService.LoginAsync(request);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl ?? "/");
            }

            return LocalRedirect($"/login?error=Invalid login attempt&returnUrl={Uri.EscapeDataString(returnUrl ?? "/")}");
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] ApplicationUser user, [FromForm] string password, [FromQuery] string? returnUrl)
        {
            var result = await authService.RegisterAsync(user, password);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl ?? "/");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout([FromQuery] string? returnUrl)
        {
            await authService.LogoutAsync();
            return LocalRedirect(returnUrl ?? "/");
        }
    }
}
