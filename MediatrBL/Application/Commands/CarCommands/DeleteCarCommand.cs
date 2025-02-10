using MediatR;

namespace MediatrBL.Application.Commands.CarCommands
{
    /// <summary>
    /// Команда на удаление автомобиля
    /// </summary>
    /// <param name="Id">Идентификатор удаляемого автомобиля</param>
    public record DeleteCarCommand(int Id) : IRequest;
}
