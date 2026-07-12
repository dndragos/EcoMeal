using BlazorApp1.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace BlazorApp1.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<SignInResult> LoginAsync(LoginRequest request);
        public Task<IdentityResult> RegisterAsync(ApplicationUser user, string password);
        public Task LogoutAsync();
    }
}
