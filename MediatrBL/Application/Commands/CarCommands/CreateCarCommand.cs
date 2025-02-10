using CarStockBLL.DTO.Car;
using CarStockDAL.Models;
using MediatR;

namespace MediatrBL.Application.Commands.CarCommands
{
    /// <summary>
    /// Команда для создания нового автомобиля
    /// </summary>
    /// <param name="Car">Создаваемый автомобиль</param>
    public record CreateCarCommand(Car Car) : IRequest<CarDTO>;
}
