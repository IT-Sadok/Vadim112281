using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;
using PrivateHospitals.Application.Dtos.Patient;
using PrivateHospitals.Application.Dtos.User;
using PrivateHospitals.Application.Services.Patient;
using PrivateHospitals.Core.Models;
using PrivateHospitals.Data.Interfaces.Patient;
using PrivateHospitals.Data.Interfaces.User;
using Xunit;

namespace PrivateHospitals.Tests.ServicesTests.Patient;

public class PatientServiceTest
{
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IValidator<RegisterDto>> _registerDtoValidatorMock;
    private readonly Mock<IValidator<RegisterPatientDto>> _registerPatientDtoValidatorMock;
    private readonly PatientService _patientService;

    public PatientServiceTest()
    {
        _patientRepositoryMock = new Mock<IPatientRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _registerDtoValidatorMock = new Mock<IValidator<RegisterDto>>();
        _registerPatientDtoValidatorMock = new Mock<IValidator<RegisterPatientDto>>();

        _patientService = new PatientService(
            _patientRepositoryMock.Object,
            _registerDtoValidatorMock.Object,
            _registerPatientDtoValidatorMock.Object,
            _userRepositoryMock.Object
            );
    }

    [Fact]
    public async Task CreatePatientAsync_ShouldReturnSuccess_WhenAllStepsAreSuccessful()
    {
        var patientDto = new RegisterPatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            UserName = "johndoe",
            Password = "Password123!"
        };
        
         _registerPatientDtoValidatorMock.Setup(x => x.ValidateAsync(patientDto, default))
            .ReturnsAsync(new ValidationResult());    
         
         _patientRepositoryMock.Setup(x => x.AddPatientAsync(It.IsAny<Core.Models.Patient>()))
             .ReturnsAsync(true);   
         
         var registerDto = new RegisterDto
         {
             FirstName = "John",
             LastName = "Doe",
             Email = "john.doe@example.com",
             Username = "johndoe",
             Password = "Password123!"
         };
         
         _registerDtoValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<RegisterDto>(), default))
             .ReturnsAsync(new ValidationResult());

         _userRepositoryMock.Setup(x => x.CreateUserAsync(It.IsAny<AppUser>(), patientDto.Password))
             .ReturnsAsync(IdentityResult.Success);

         _userRepositoryMock.Setup(x => x.AddUserToRoleAsync(It.IsAny<AppUser>(), "Patient"))
             .ReturnsAsync(IdentityResult.Success);         
         
         var result = await _patientService.CreatePatientAsync(patientDto);
         
         result.Success.Should().BeTrue();
         result.Data.Should().BeTrue();
         result.Errors.Should().BeEmpty();
    }
    
        [Fact]
        public async Task CreatePatientAsync_ShouldReturnError_WhenPatientValidationFails()
        {
            var patientDto = new RegisterPatientDto();
            var validationErrors = new List<ValidationFailure> { new ValidationFailure("FirstName", "First name is required.") };
            _registerPatientDtoValidatorMock.Setup(v => v.ValidateAsync(patientDto, default))
                .ReturnsAsync(new ValidationResult(validationErrors));
            
            var result = await _patientService.CreatePatientAsync(patientDto);
            
            result.Success.Should().BeFalse();
            result.Data.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error == "First name is required.");
        }
        
        [Fact]
        public async Task CreatePatientAsync_ShouldReturnError_WhenAddingPatientFails()
        {
            var patientDto = new RegisterPatientDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe",
                Password = "Password123!"
            };

            _registerPatientDtoValidatorMock.Setup(v => v.ValidateAsync(patientDto, default))
                .ReturnsAsync(new ValidationResult());

            _patientRepositoryMock.Setup(repo => repo.AddPatientAsync(It.IsAny<Core.Models.Patient>()))
                .ReturnsAsync(false);
            
            var result = await _patientService.CreatePatientAsync(patientDto);
            
            result.Success.Should().BeFalse();
            result.Data.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error == "Error creating patient.");
        }
        
        [Fact]
        public async Task CreatePatientAsync_ShouldReturnError_WhenUserCreationFails()
        {
            var patientDto = new RegisterPatientDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe",
                Password = "Password123!"
            };

            _registerPatientDtoValidatorMock.Setup(v => v.ValidateAsync(patientDto, default))
                .ReturnsAsync(new ValidationResult());

            _patientRepositoryMock.Setup(repo => repo.AddPatientAsync(It.IsAny<Core.Models.Patient>()))
                .ReturnsAsync(true);

            _registerDtoValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegisterDto>(), default))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock.Setup(repo => repo.CreateUserAsync(It.IsAny<AppUser>(), patientDto.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to create user." }));
            
            var result = await _patientService.CreatePatientAsync(patientDto);
            
            result.Success.Should().BeFalse();
            result.Data.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error == "Failed to create user.");
        }
        
        [Fact]
        public async Task CreatePatientAsync_ShouldReturnError_WhenAddingRoleFails()
        {
            var patientDto = new RegisterPatientDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe",
                Password = "Password123!"
            };

            _registerPatientDtoValidatorMock.Setup(v => v.ValidateAsync(patientDto, default))
                .ReturnsAsync(new ValidationResult());

            _patientRepositoryMock.Setup(repo => repo.AddPatientAsync(It.IsAny<Core.Models.Patient>()))
                .ReturnsAsync(true);

            _registerDtoValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegisterDto>(), default))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock.Setup(repo => repo.CreateUserAsync(It.IsAny<AppUser>(), patientDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userRepositoryMock.Setup(repo => repo.AddUserToRoleAsync(It.IsAny<AppUser>(), "Patient"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to add role." }));
            
            var result = await _patientService.CreatePatientAsync(patientDto);
            
            result.Success.Should().BeFalse();
            result.Data.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error == "Failed to add role.");
        }
}
