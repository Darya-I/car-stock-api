using CarStockBLL.DTO.User;
using MediatR;

namespace MediatrBL.Application.Queries.UserQueries
{
    /// <summary>
    /// Запрос на получение коллекции пользователей
    /// </summary>
    public record GetAllUsersQuery : IRequest<List<GetUserDTO>>;
}
