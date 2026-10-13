using FluentValidation;

namespace users_service.Users.Requests;

public class CreateUserRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }


    public User To()
    {
        return new User
        {
            FirstName = this.FirstName,
            LastName = this.LastName,
        };
    }
}

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(100)
            .WithMessage("First name cannot exceed 100 characters");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(100)
            .WithMessage("Last name cannot exceed 100 characters");
    }
}