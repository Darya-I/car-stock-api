using FluentValidation;
using MediatrBL.Application.Commands.UserCommands;

namespace MediatrBL.Application.Validators.Users
{
    /// <summary>
    /// Валидатор на команду обновление аккаунта пользователя
    /// </summary>
    public sealed class UpdateUserAccountCommandValidator : AbstractValidator<UpdateUserAccountCommand>
    {
        public UpdateUserAccountCommandValidator() 
        {
            RuleFor(command => command.User.UserName)
                .Matches(@"^[A-Za-z]+$").WithMessage("Your username must contain only English letters.");

            RuleFor(command => command.User.PasswordHash)
                .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");
        }
    }
}
