using CarStockDAL.Models;
using MediatR;

namespace MediatrBL.Application.Commands.UserCommands
{
    /// <summary>
    /// Команда на вход/регистрацию через Гугл
    /// </summary>
    /// <param name="User">Пользователь</param>
    public record ProccessGoogleCommand(User User) : IRequest<string>;
}
