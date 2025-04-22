using Microsoft.AspNetCore.Identity;
using RO.DevTest.Application.Contracts.Infrastructure;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Enums;

namespace RO.DevTest.Infrastructure.Abstractions;

/// <summary>
/// This is an abstraction of the Identity library, creating methods that will interact with 
/// it to create and update users
/// </summary>
public class IdentityAbstractor(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    RoleManager<IdentityRole> roleManager
    ) : IIdentityAbstractor 
{
    public async Task<User?> FindUserByEmailAsync(string email) => await userManager.FindByEmailAsync(email);

    public async Task<User?> FindUserByIdAsync(string userId) => await userManager.FindByIdAsync(userId);

    public async Task<IList<string>> GetUserRolesAsync(User user) => await userManager.GetRolesAsync(user);

    public async Task<IdentityResult> CreateUserAsync(User partnerUser, string password) {
        if(string.IsNullOrEmpty(password)) {
            throw new ArgumentException($"{nameof(password)} cannot be null or empty", nameof(password));
        }

        if(string.IsNullOrEmpty(partnerUser.Email)) {
            throw new ArgumentException($"{nameof(User.Email)} cannot be null or empty", nameof(partnerUser));
        }

        return await userManager.CreateAsync(partnerUser, password);
    }

    public async Task<SignInResult> PasswordSignInAsync(User user, string password)
        => await signInManager.PasswordSignInAsync(user, password, false, false);

    public async Task<IdentityResult> DeleteUser(User user) => await userManager.DeleteAsync(user);

    public async Task<IdentityResult> AddToRoleAsync(User user, UserRoles role) {
        if(await roleManager.RoleExistsAsync(role.ToString()) is false) {
            var identityRole = new IdentityRole { Name = role.ToString() };
            await roleManager.CreateAsync(identityRole);
        }

        return await userManager.AddToRoleAsync(user, role.ToString());
    }

    public Task<User?> FindUserByUserNameAsync(string userName)
    {
        if (string.IsNullOrEmpty(userName))
            throw new ArgumentException($"{nameof(userName)} cannot be null or empty", nameof(userName));
        
        return userManager.FindByNameAsync(userName);
    }
}
