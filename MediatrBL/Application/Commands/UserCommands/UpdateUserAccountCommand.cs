using CarStockBLL.DTO.User;
using CarStockDAL.Models;
using MediatR;

namespace MediatrBL.Application.Commands.UserCommands
{
    /// <summary>
    /// Команда на обновление информации аккаунта пользователя
    /// </summary>
    /// <param name="User">Пользователь</param>
    public record UpdateUserAccountCommand(User User) : IRequest<GetUserDTO>;
}
