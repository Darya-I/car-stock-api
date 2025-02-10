using CarStockBLL.DTO.Auth;
using CarStockDAL.Models;
using MediatR;

namespace MediatrBL.Application.Commands.UserCommands
{
    /// <summary>
    /// Команда на обновление refresh-токена
    /// </summary>
    /// <param name="User">Пользователь</param>
    public record UpdateRefreshTokenCommand(User User) : IRequest<RefreshTokenResponse>;
}
