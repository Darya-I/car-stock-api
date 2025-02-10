using MediatR;

namespace MediatrBL.Application.Commands.UserCommands
{
    /// <summary>
    /// Команда на удаление пользователя
    /// </summary>
    /// <param name="Email">Почта пользователя</param>
    public record DeleteUserCommand(string Email) : IRequest;
}
