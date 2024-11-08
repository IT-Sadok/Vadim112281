using FluentValidation;
using FluentValidation.Results;
using PrivateHospitals.Application.Dtos.Doctor;
using PrivateHospitals.Application.Dtos.Patient;
using PrivateHospitals.Application.Dtos.User;
using PrivateHospitals.Application.Interfaces.Doctor;
using PrivateHospitals.Application.Responses;
using PrivateHospitals.Core.Models;
using PrivateHospitals.Data.Interfaces.Doctor;
using PrivateHospitals.Data.Interfaces.Patient;
using PrivateHospitals.Data.Interfaces.User;

namespace PrivateHospitals.Application.Services.Doctor;

public class DoctorService: IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IValidator<RegisterDto> _registerDtoValidator;
    private readonly IValidator<RegisterDoctorDto> _registerDoctorDtoValidator;
    private readonly IUserRepository _userRepository;

    public DoctorService(IDoctorRepository doctorRepository, IValidator<RegisterDto> registerDtoValidator, IValidator<RegisterDoctorDto> registerDoctorDtoValidator, IUserRepository userRepository)
    {
        _doctorRepository = doctorRepository;
        _registerDtoValidator = registerDtoValidator;
        _registerDoctorDtoValidator = registerDoctorDtoValidator;
        _userRepository = userRepository;
    }


    public async Task<ServiceResponse<bool>> RegisterDoctorAsync(RegisterDoctorDto doctorDto)
    {   
        //Register to Doctors
        ValidationResult validationDoctorResult = await _registerDoctorDtoValidator.ValidateAsync(doctorDto);
        
        if (!validationDoctorResult.IsValid)
        {
            var errors = validationDoctorResult.Errors
                .Select(error => error.ErrorMessage)
                .ToList();

            return ServiceResponse<bool>.ErrorResponse(errors);
        }
        
        Core.Models.Doctor doctor = new Core.Models.Doctor()
        {
            FirstName = doctorDto.FirstName,
            Email = doctorDto.Email,
            LastName = doctorDto.LastName,
            UserName = doctorDto.UserName,
            Speciality = doctorDto.Speciality
        };

        var doctorResult = await _doctorRepository.AddDoctorAsync(doctor);
        
        if (doctorResult == false)
        {
            return ServiceResponse<bool>.ErrorResponse(new List<string>() { "Error creating doctor." });
        }
        
        //Register To Users
        RegisterDto registerDto = new RegisterDto()
        {
            FirstName = doctorDto.FirstName,
            LastName = doctorDto.LastName,
            Email = doctorDto.Email,
            Username = doctorDto.UserName,
            Password = doctorDto.Password
        };
        
        ValidationResult validationUserResult = await _registerDtoValidator.ValidateAsync(registerDto);
        if (!validationUserResult.IsValid)
        {
            var errors = validationUserResult.Errors.Select(e => e.ErrorMessage).ToList();

            return ServiceResponse<bool>.ErrorResponse(errors);
        }
        
        var user = new AppUser()
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            UserName = registerDto.Username,
            Email = registerDto.Email
        };
        
        var identityResult = await _userRepository.CreateUserAsync(user, registerDto.Password);
        
        if (!identityResult.Succeeded)
        {
            var errors = identityResult.Errors.Select(e => e.Description).ToList();
            return ServiceResponse<bool>.ErrorResponse(errors);
        }
        
        var roleResult =  await _userRepository.AddUserToRoleAsync(user, "Doctor");
        
        if (!roleResult.Succeeded)
        {
            var errors = roleResult.Errors.Select(e => e.Description).ToList();
            return ServiceResponse<bool>.ErrorResponse(errors);
        }
        
        return ServiceResponse<bool>.SuccessResponse(true);
        
    }
}