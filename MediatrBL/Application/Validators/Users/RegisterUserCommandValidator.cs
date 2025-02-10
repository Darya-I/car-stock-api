using FluentValidation;
using MediatrBL.Application.Commands.UserCommands;

namespace MediatrBL.Application.Validators.Users
{
    /// <summary>
    /// Валидатор на команду регистрации пользователя
    /// </summary>
    public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator() 
        {
            RuleFor(command =>  command.User.Email)
                .NotEmpty()
                .WithMessage("The email cannot be empty");

            RuleFor(command => command.User.UserName)
                .NotEmpty().WithMessage("The username cannot be empty")
                .Matches(@"^[A-Za-z]+$").WithMessage("Your username must contain only English letters.");

            RuleFor(command => command.User.PasswordHash)
                .NotEmpty().WithMessage("Your password cannot be empty")
                .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");
        }
    }
}