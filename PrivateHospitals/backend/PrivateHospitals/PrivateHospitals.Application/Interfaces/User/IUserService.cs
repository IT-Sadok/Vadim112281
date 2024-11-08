using Microsoft.AspNetCore.Identity;
using PrivateHospitals.Application.Dtos.User;
using PrivateHospitals.Application.Responses;

namespace PrivateHospitals.Application.Interfaces.User;

public interface IUserService
{
    // Task<ServiceResponse<bool>> RegisterUserAsync(RegisterDto registerDto, string role);

    Task<ServiceResponse<UserLoggedDto>> LoginUserAsync(LoginDto loginDto);
}