using FluentValidation;
using MediatrBL.Application.Commands.UserCommands;

namespace MediatrBL.Application.Validators.Users
{
    /// <summary>
    /// Валидатор на команду создания пользователя
    /// </summary>
    public sealed class CreateUserCommandValidator : AbstractValidator<CreareUserCommand>
    {
        public CreateUserCommandValidator() 
        {
            RuleFor(command => command.CreateUserDTO.Email)
                .NotEmpty()
                .WithMessage("The email cannot be empty");

            RuleFor(command => command.CreateUserDTO.RoleId)
                .NotEmpty()
                .WithMessage("The role identifier cannot be empty");

            RuleFor(command => command.CreateUserDTO.Password)
                .NotEmpty().WithMessage("The password cannot be empty")
                .MinimumLength(8).WithMessage("The password length must be at least 8.")
                .MaximumLength(16).WithMessage("The password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("The password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("The password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("The password must contain at least one number.")
                .Matches(@"[\!\?\*\.]+").WithMessage("The password must contain at least one (!? *.).");
        }
    }
}
