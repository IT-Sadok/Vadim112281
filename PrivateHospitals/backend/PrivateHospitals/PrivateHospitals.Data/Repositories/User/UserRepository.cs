using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrivateHospitals.Core.Models;
using PrivateHospitals.Data.Interfaces.User;

namespace PrivateHospitals.Data.Repositories.User;

public class UserRepository: IUserRepository
{
    private readonly UserManager<AppUser> _userManager;

    public UserRepository(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AppUser?> GetUserByEmailAsync(string email)
    {
        return await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> AddUserToRoleAsync(AppUser user, string roleName)
    {
        return await _userManager.AddToRoleAsync(user, roleName);
    }
}