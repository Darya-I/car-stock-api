using FluentValidation;
using MediatrBL.Application.Commands.UserCommands;

namespace MediatrBL.Application.Validators.Users
{
    /// <summary>
    /// Валидатор на команду авторизации пользователя
    /// </summary>
    public sealed class AuthUserCommandValidator : AbstractValidator<AuthUserCommand>
    {
        public AuthUserCommandValidator() 
        {
            RuleFor(command => command.User.Email)
                .NotEmpty()
                .WithMessage("The email cannot be empty");

            RuleFor(command => command.User.PasswordHash)
                .NotEmpty()
                .WithMessage("Your password cannot be empty");
        }
    }
}
