using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using PrivateHospitals.Application.Dtos.User;
using PrivateHospitals.Application.Interfaces;
using PrivateHospitals.Application.Interfaces.User;
using PrivateHospitals.Application.Responses;
using PrivateHospitals.Core.Models;
using PrivateHospitals.Data.Interfaces.User;

namespace PrivateHospitals.Application.Services.User;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<RegisterDto> _registerDtoValidator;
    private readonly IValidator<LoginDto> _loginDtoValidator;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;

    public UserService(IUserRepository userRepository, IValidator<RegisterDto> registerDtoValidator, SignInManager<AppUser> signInManager, IValidator<LoginDto> loginDtoValidator, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _registerDtoValidator = registerDtoValidator;
        _signInManager = signInManager;
        _loginDtoValidator = loginDtoValidator;
        _tokenService = tokenService;
    }
    
    public async Task<ServiceResponse<UserLoggedDto>> LoginUserAsync(LoginDto loginDto)
    {
        ValidationResult validationResult = await _loginDtoValidator.ValidateAsync(loginDto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return ServiceResponse<UserLoggedDto>.ErrorResponse(errors);
        }

        var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);

        if (user is null)
        {
            return ServiceResponse<UserLoggedDto>.ErrorResponse(new List<string> { "User not found" });
        }
        
        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        
        if (!signInResult.Succeeded)
        {
            return ServiceResponse<UserLoggedDto>.ErrorResponse(new List<string> { "Invalid login or password" });
        }
        
        return ServiceResponse<UserLoggedDto>.SuccessResponse(new UserLoggedDto
        {
            Email = loginDto.Email,
            Token = _tokenService.CreateToken(user)
        });
    }
}