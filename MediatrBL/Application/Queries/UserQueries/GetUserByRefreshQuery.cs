using CarStockDAL.Models;
using MediatR;

namespace MediatrBL.Application.Queries.UserQueries
{
    /// <summary>
    /// Запрос на получение пользователя по refresh-токену
    /// </summary>
    /// <param name="RefreshToken">Токен обновления</param>
    public record GetUserByRefreshQuery(string RefreshToken) : IRequest<User>;
}
