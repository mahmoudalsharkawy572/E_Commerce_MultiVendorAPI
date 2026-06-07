using FluentValidation;

namespace ECommerce.Application.Authentication.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(x => x.Password)
            .Matches(@"[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.");

            RuleFor(x => x.Password)
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.");

            RuleFor(x => x.Password)
                .Matches(@"\d")
                .WithMessage("Password must contain at least one number.");
        }
    }
}
