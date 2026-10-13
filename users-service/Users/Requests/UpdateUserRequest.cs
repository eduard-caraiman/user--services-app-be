using FluentValidation;

namespace users_service.Users.Requests;

public class UpdateUserRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public void To(User user)
    {
        user.FirstName = this.FirstName;
        user.LastName = this.LastName;
    }
}

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
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