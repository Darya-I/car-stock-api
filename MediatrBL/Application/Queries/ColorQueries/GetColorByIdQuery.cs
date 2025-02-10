using CarStockDAL.Models;
using MediatR;

namespace MediatrBL.Application.Queries.ColorQueries
{
    /// <summary>
    /// Запрос на получение цвета автомобиля
    /// </summary>
    /// <param name="Id">Идентификатор</param>
    public record GetColorByIdQuery(int Id) : IRequest<Color>;
}