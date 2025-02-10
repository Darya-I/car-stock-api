using CarStockBLL.DTO.User;
using MediatR;

namespace MediatrBL.Application.Commands.UserCommands
{
    /// <summary>
    /// Команда на обновление пользователя
    /// </summary>
    /// <param name="UpdateUserDTO">DTO обновления пользователя</param>
    public record UpdateUserCommand(UpdateUserDTO UpdateUserDTO) : IRequest<GetUserDTO>;
}
