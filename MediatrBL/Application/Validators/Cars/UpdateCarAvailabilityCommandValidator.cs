using FluentValidation;
using MediatrBL.Application.Commands.CarCommands;

namespace MediatrBL.Application.Validators.Cars
{
    /// <summary>
    /// Валидатор на команду обновление доступности автомобиля
    /// </summary>
    public sealed class UpdateCarAvailabilityCommandValidator : AbstractValidator<UpdateCarAvailabilityCommand>
    {
        public UpdateCarAvailabilityCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty()
                .WithMessage("The car identifier cannot be empty");

            RuleFor(command => command.IsAvailable)
                .NotEmpty()
                .WithMessage($"The availability of car cannot be empty");
        }
    }
}
