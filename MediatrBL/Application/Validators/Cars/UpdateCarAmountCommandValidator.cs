using FluentValidation;
using MediatrBL.Application.Commands.CarCommands;

namespace MediatrBL.Application.Validators.Cars
{
    /// <summary>
    /// Валидатор на команду обновление количества автомобиля
    /// </summary>
    public sealed class UpdateCarAmountCommandValidator : AbstractValidator<UpdateCarAmountCommand>
    {
        public UpdateCarAmountCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty()
                .WithMessage("The car identifier cannot be empty");

            RuleFor(command => command.Amount)
                .NotEmpty()
                .WithMessage("The amount of car cannot be empty");
        }
    }
}