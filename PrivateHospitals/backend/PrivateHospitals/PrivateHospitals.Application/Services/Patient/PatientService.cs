using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using PrivateHospitals.Application.Dtos.Patient;
using PrivateHospitals.Application.Dtos.User;
using PrivateHospitals.Application.Interfaces.Patient;
using PrivateHospitals.Application.Responses;
using PrivateHospitals.Core.Models;
using PrivateHospitals.Data.Interfaces.Patient;
using PrivateHospitals.Data.Interfaces.User;

namespace PrivateHospitals.Application.Services.Patient;

public class PatientService: IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IValidator<RegisterDto> _registerDtoValidator;
    private readonly IValidator<RegisterPatientDto> _registerPatientDtoValidator;
    private readonly IUserRepository _userRepository;
    
    public PatientService(IPatientRepository patientRepository, IValidator<RegisterDto> registerDtoValidator, IValidator<RegisterPatientDto> registerPatientDtoValidator, IUserRepository userRepository)
    {
        _patientRepository = patientRepository;
        _registerDtoValidator = registerDtoValidator;
        _registerPatientDtoValidator = registerPatientDtoValidator;
        _userRepository = userRepository;
    }

    public async Task<ServiceResponse<bool>> CreatePatientAsync(RegisterPatientDto patientDto)
    {
        //Register to Patients
        ValidationResult validationPatientResult = await _registerPatientDtoValidator.ValidateAsync(patientDto);

        if (!validationPatientResult.IsValid)
        {
            var errors = validationPatientResult.Errors
                .Select(error => error.ErrorMessage)
                .ToList();

            return ServiceResponse<bool>.ErrorResponse(errors);
        }
        
        Core.Models.Patient patient = new Core.Models.Patient()
        {
            FirstName = patientDto.FirstName,
            Email = patientDto.Email,
            LastName = patientDto.LastName,
            UserName = patientDto.UserName
        };
        
        var patientResult = await _patientRepository.AddPatientAsync(patient);

        if (patientResult == false)
        {
            return ServiceResponse<bool>.ErrorResponse(new List<string>() { "Error creating patient." });
        }
        
        
        //Register To Users
        RegisterDto registerDto = new RegisterDto()
        {
            FirstName = patientDto.FirstName,
            LastName = patientDto.LastName,
            Email = patientDto.Email,
            Username = patientDto.UserName,
            Password = patientDto.Password
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
        
        var roleResult =  await _userRepository.AddUserToRoleAsync(user, "Patient");

        if (!roleResult.Succeeded)
        {
            var errors = roleResult.Errors.Select(e => e.Description).ToList();
            return ServiceResponse<bool>.ErrorResponse(errors);
        }
        
        return ServiceResponse<bool>.SuccessResponse(true);
    }
}