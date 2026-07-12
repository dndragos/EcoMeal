using BlazorApp1.Entities;
using Microsoft.AspNetCore.Identity;
using BlazorApp1.Services.Interfaces;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity.Data;
using BlazorApp1.Constants;

namespace BlazorApp1.Services
{
    public class AuthService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : IAuthService
    {
        public async Task<SignInResult> LoginAsync(LoginRequest request) {
            return await signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
        }

        public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password)
        {
            user.UserName = user.Email;
            user.EmailConfirmed = true;

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, AppRoles.Customer);
                await signInManager.SignInAsync(user, isPersistent: false);
            }
            return result;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}
