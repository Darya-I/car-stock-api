using FluentValidation;
using MediatrBL.Application.Commands.UserCommands;

namespace MediatrBL.Application.Validators.Users
{
    /// <summary>
    /// Валидатор на команду обновление refresh-токена пользователя
    /// </summary>
    public sealed class UpdateRefreshTokenCommandValidator : AbstractValidator<UpdateRefreshTokenCommand>
    {
        public UpdateRefreshTokenCommandValidator() 
        {
            RuleFor(command => command.User.RefreshToken)
                .NotEmpty()
                .WithMessage("The refresh token cannot be empty");
        }
    }
}
