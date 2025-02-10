using CarStockBLL.DTO.Car;
using MediatR;

namespace MediatrBL.Application.Commands.CarCommands
{
    /// <summary>
    /// Команда на обновление количества автомобиля
    /// </summary>
    /// <param name="Id">Идентификатор обновляемого автомобиля</param>
    /// <param name="Amount">Количество</param>
    public record UpdateCarAmountCommand(int Id, int Amount) : IRequest<CarAmountDTO>;
}
