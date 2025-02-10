using CarStockBLL.DTO.Auth;
using CarStockDAL.Models;
using MediatR;

namespace MediatrBL.Application.Commands.UserCommands
{
    /// <summary>
    /// Команда на аутентификацию пользователя
    /// </summary>
    /// <param name="User">Пользователь</param>
    public record AuthUserCommand(User User) : IRequest<AuthResponse>;
}
