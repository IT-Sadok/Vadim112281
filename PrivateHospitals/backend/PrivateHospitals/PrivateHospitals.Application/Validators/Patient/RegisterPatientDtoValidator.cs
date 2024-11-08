using FluentValidation;
using PrivateHospitals.Application.Dtos.Patient;

namespace PrivateHospitals.Application.Validators.Patient;

public class RegisterPatientDtoValidator: AbstractValidator<RegisterPatientDto>
{
    public RegisterPatientDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty")
            .Matches(@"^[^\d]+$").WithMessage("First name cannot contain numbers");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty")
            .Matches(@"^[^\d]+$").WithMessage("Last name cannot contain numbers");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Invalid email address");
        
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName cannot be empty")
            .Length(5, 50).WithMessage("UserName must be between 5 and 50 characters"); 
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character (e.g., !, @, #, etc.)");
    }
}