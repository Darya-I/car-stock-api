using CarStockBLL.DTO.User;
using MediatR;

namespace MediatrBL.Application.Queries.UserQueries
{
    /// <summary>
    /// Запрос на получение пользователя
    /// </summary>
    /// <param name="Email">Почта пользователя</param>
    public record GetUserByEmailQuery(string Email) : IRequest<GetUserDTO>;
}
