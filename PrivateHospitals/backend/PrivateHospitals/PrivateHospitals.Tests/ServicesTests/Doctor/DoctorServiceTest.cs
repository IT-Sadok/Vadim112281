using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;
using PrivateHospitals.Application.Dtos.Doctor;
using PrivateHospitals.Application.Dtos.User;
using PrivateHospitals.Application.Services.Doctor;
using PrivateHospitals.Core.Models;
using PrivateHospitals.Data.Interfaces.Doctor;
using PrivateHospitals.Data.Interfaces.User;
using Xunit;

public class DoctorServiceTest
{
    private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IValidator<RegisterDto>> _registerDtoValidatorMock;
    private readonly Mock<IValidator<RegisterDoctorDto>> _registerDoctorDtoValidatorMock;
    private readonly DoctorService _doctorService;

    public DoctorServiceTest()
    {
        _doctorRepositoryMock = new Mock<IDoctorRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _registerDtoValidatorMock = new Mock<IValidator<RegisterDto>>();
        _registerDoctorDtoValidatorMock = new Mock<IValidator<RegisterDoctorDto>>();

        _doctorService = new DoctorService(
            _doctorRepositoryMock.Object,
            _registerDtoValidatorMock.Object,
            _registerDoctorDtoValidatorMock.Object,
            _userRepositoryMock.Object);
    }

    [Fact]
    public async Task RegisterDoctorAsync_ShouldReturnSuccess_WhenAllStepsAreSuccessful()
    {
        var doctorDto = new RegisterDoctorDto
        {
            FirstName = "Alice",
            LastName = "Smith",
            Email = "alice.smith@example.com",
            UserName = "alicesmith",
            Password = "Password123!",
            Speciality = "Cardiology"
        };

        _registerDoctorDtoValidatorMock.Setup(v => v.ValidateAsync(doctorDto, default))
            .ReturnsAsync(new ValidationResult());

        _doctorRepositoryMock.Setup(repo => repo.AddDoctorAsync(It.IsAny<Doctor>()))
            .ReturnsAsync(true);

        _registerDtoValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegisterDto>(), default))
            .ReturnsAsync(new ValidationResult());

        _userRepositoryMock.Setup(repo => repo.CreateUserAsync(It.IsAny<AppUser>(), doctorDto.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userRepositoryMock.Setup(repo => repo.AddUserToRoleAsync(It.IsAny<AppUser>(), "Doctor"))
            .ReturnsAsync(IdentityResult.Success);
        
        var result = await _doctorService.RegisterDoctorAsync(doctorDto);
        
        result.Success.Should().BeTrue();
        result.Data.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task RegisterDoctorAsync_ShouldReturnError_WhenDoctorValidationFails()
    {
        var doctorDto = new RegisterDoctorDto();
        var validationErrors = new List<ValidationFailure> { new ValidationFailure("FirstName", "First name is required.") };
        _registerDoctorDtoValidatorMock.Setup(v => v.ValidateAsync(doctorDto, default))
            .ReturnsAsync(new ValidationResult(validationErrors));
        
        var result = await _doctorService.RegisterDoctorAsync(doctorDto);
        
        result.Success.Should().BeFalse();
        result.Data.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error == "First name is required.");
    }

    [Fact]
    public async Task RegisterDoctorAsync_ShouldReturnError_WhenAddingDoctorFails()
    {
        var doctorDto = new RegisterDoctorDto
        {
            FirstName = "Alice",
            LastName = "Smith",
            Email = "alice.smith@example.com",
            UserName = "alicesmith",
            Password = "Password123!",
            Speciality = "Cardiology"
        };

        _registerDoctorDtoValidatorMock.Setup(v => v.ValidateAsync(doctorDto, default))
            .ReturnsAsync(new ValidationResult());

        _doctorRepositoryMock.Setup(repo => repo.AddDoctorAsync(It.IsAny<Doctor>()))
            .ReturnsAsync(false);
        
        var result = await _doctorService.RegisterDoctorAsync(doctorDto);
        
        result.Success.Should().BeFalse();
        result.Data.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error == "Error creating doctor.");
    }

    [Fact]
    public async Task RegisterDoctorAsync_ShouldReturnError_WhenUserCreationFails()
    {
        var doctorDto = new RegisterDoctorDto
        {
            FirstName = "Alice",
            LastName = "Smith",
            Email = "alice.smith@example.com",
            UserName = "alicesmith",
            Password = "Password123!",
            Speciality = "Cardiology"
        };

        _registerDoctorDtoValidatorMock.Setup(v => v.ValidateAsync(doctorDto, default))
            .ReturnsAsync(new ValidationResult());

        _doctorRepositoryMock.Setup(repo => repo.AddDoctorAsync(It.IsAny<Doctor>()))
            .ReturnsAsync(true);

        _registerDtoValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegisterDto>(), default))
            .ReturnsAsync(new ValidationResult());

        _userRepositoryMock.Setup(repo => repo.CreateUserAsync(It.IsAny<AppUser>(), doctorDto.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to create user." }));
        
        var result = await _doctorService.RegisterDoctorAsync(doctorDto);
        
        result.Success.Should().BeFalse();
        result.Data.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error == "Failed to create user.");
    }

    [Fact]
    public async Task RegisterDoctorAsync_ShouldReturnError_WhenAddingRoleFails()
    {
        var doctorDto = new RegisterDoctorDto
        {
            FirstName = "Alice",
            LastName = "Smith",
            Email = "alice.smith@example.com",
            UserName = "alicesmith",
            Password = "Password123!",
            Speciality = "Cardiology"
        };

        _registerDoctorDtoValidatorMock.Setup(v => v.ValidateAsync(doctorDto, default))
            .ReturnsAsync(new ValidationResult());

        _doctorRepositoryMock.Setup(repo => repo.AddDoctorAsync(It.IsAny<Doctor>()))
            .ReturnsAsync(true);

        _registerDtoValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegisterDto>(), default))
            .ReturnsAsync(new ValidationResult());

        _userRepositoryMock.Setup(repo => repo.CreateUserAsync(It.IsAny<AppUser>(), doctorDto.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userRepositoryMock.Setup(repo => repo.AddUserToRoleAsync(It.IsAny<AppUser>(), "Doctor"))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to add role." }));
        
        var result = await _doctorService.RegisterDoctorAsync(doctorDto);
        
        result.Success.Should().BeFalse();
        result.Data.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error == "Failed to add role.");
    }
}
