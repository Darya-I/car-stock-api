using CarStockDAL.Models;
using MediatR;

namespace MediatrBL.Application.Queries.CarModelQueries
{
    /// <summary>
    /// Запрос на получение модели автомобиля
    /// </summary>
    /// <param name="Id">Идентификатор модели</param>
    public record GetCarModelByIdQuery(int Id) : IRequest<CarModel>;
}
