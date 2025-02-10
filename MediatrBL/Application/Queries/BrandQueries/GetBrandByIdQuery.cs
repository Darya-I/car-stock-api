using CarStockDAL.Models;
using MediatR;

namespace MediatrBL.Application.Queries.BrandQueries
{
    /// <summary>
    /// Запрос на получение марки автомобиля
    /// </summary>
    /// <param name="Id">Идентификатор марки</param>
    public record GetBrandByIdQuery(int Id) : IRequest<Brand>;
}
