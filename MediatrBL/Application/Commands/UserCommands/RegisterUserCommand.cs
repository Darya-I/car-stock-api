using CarStockBLL.DTO.User;
using CarStockDAL.Models;
using MediatR;

namespace MediatrBL.Application.Commands.UserCommands
{
    /// <summary>
    /// Команда на регистрацию пользователя
    /// </summary>
    /// <param name="User">Пользователь</param>
    public record RegisterUserCommand(User User) : IRequest<GetUserDTO>;
}
