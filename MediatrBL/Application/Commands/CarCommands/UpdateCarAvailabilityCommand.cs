using CarStockBLL.DTO.Car;
using MediatR;

namespace MediatrBL.Application.Commands.CarCommands
{
    /// <summary>
    /// Команда на обновление доступности автомобиля
    /// </summary>
    /// <param name="Id">Идентификатор обновляемого автомобиля</param>
    /// <param name="IsAvailable">Доступность</param>
    public record UpdateCarAvailabilityCommand(int Id, bool IsAvailable) : IRequest<CarAvailabilityDTO>;
}
