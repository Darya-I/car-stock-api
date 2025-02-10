﻿using FluentValidation;
using MediatrBL.Application.Commands.CarCommands;

namespace MediatrBL.Application.Validators.Cars
{
    /// <summary>
    /// Валидатор на команду создание автомобиля
    /// </summary>
    public sealed class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
    {
        public CreateCarCommandValidator() 
        {
            RuleFor(command => command.Car.Id)
                .NotEmpty()
                .WithMessage("The car identifier cannot be empty");

            RuleFor(command => command.Car.BrandId)
                .NotEmpty()
                .WithMessage("The brand identifier cannot be empty");

            RuleFor(command => command.Car.CarModelId)
                .NotEmpty()
                .WithMessage("The model identifier cannot be empty");

            RuleFor(command => command.Car.ColorId)
                .NotEmpty()
                .WithMessage("The color identifier cannot be empty");

            RuleFor(command => command.Car.IsAvailable)
                .NotEmpty()
                .WithMessage($"The availability of car cannot be empty");

            RuleFor(command => command.Car.Amount)
                .NotEmpty()
                .WithMessage("The amount of car cannot be empty");
        }
    }
}
