using JackSite.Domain.Repositories;

namespace JackSite.Application.Features.Users.CreateUser;

public class CreateUserValidator:AbstractValidator<CreateUserBasicCommand>
{
    public CreateUserValidator(IUserBasicRepository userBasicRepository)
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email address")
            .NotEmpty()
            .MustAsync(async (email, cancellationToken) =>
            {
                var existingUser = await userBasicRepository
                    .ExistsAsync(x=>x.Email == email, cancellationToken);
                return existingUser;
            })
            .WithMessage("Email is already registered");
        
        RuleFor(x=>x.UserName)
            .NotEmpty()
            .Length(6,20)
            .WithMessage("Username must be between 6 and 20 characters")
            .MustAsync(async (username, cancellationToken) =>
            {
                var usernameIsExist = await userBasicRepository.ExistsAsync(x=>x.Username == username, cancellationToken);
                return usernameIsExist;
            })
            .WithMessage("Username is already taken");
        
        RuleFor(x => x.Password)
            .Length(8, 20)
            .WithMessage("Password must be between 8 and 20 characters")
            .Must(BeAValidPassword)
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character");
    }
    private static bool BeAValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;

        var hasUpper = Regex.IsMatch(password, "[A-Z]");
        var hasLower = Regex.IsMatch(password, "[a-z]");
        var hasDigit = Regex.IsMatch(password, "[0-9]");
        var hasSpecial = Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]");

        return hasUpper && hasLower && hasDigit && hasSpecial;
    }
}