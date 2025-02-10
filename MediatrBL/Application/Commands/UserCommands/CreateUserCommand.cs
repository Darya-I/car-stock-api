using CarStockBLL.DTO.User;
using MediatR;

namespace MediatrBL.Application.Commands.UserCommands
{
    /// <summary>
    /// Команда на создание пользователя
    /// </summary>
    /// <param name="CreateUserDTO">DTO создаваемого пользователя</param>
    public record CreareUserCommand(CreateUserDTO CreateUserDTO) : IRequest<GetUserDTO>;
}
