using FluentValidation;
using PrivateHospitals.Application.Dtos.User;

namespace PrivateHospitals.Application.Validations.User;

public class RegisterDtoValidator: AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty")
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty")
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Invalid email address");
        
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username cannot be empty")
            .Length(5, 50).WithMessage("Username must be between 5 and 50 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character (e.g., !, @, #, etc.)");
    }
}