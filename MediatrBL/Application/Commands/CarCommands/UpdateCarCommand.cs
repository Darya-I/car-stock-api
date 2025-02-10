using CarStockBLL.DTO.Car;
using CarStockDAL.Models;
using MediatR;

namespace MediatrBL.Application.Commands.CarCommands
{
    /// <summary>
    /// Команда на обновление автомобиля
    /// </summary>
    /// <param name="Car">Автомобиль</param>
    public record UpdateCarCommand(Car Car) : IRequest<CarDTO>;
}
