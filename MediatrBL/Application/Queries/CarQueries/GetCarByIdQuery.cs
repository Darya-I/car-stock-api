using CarStockBLL.DTO.Car;
using MediatR;

namespace MediatrBL.Application.Queries.CarQueries
{
    /// <summary>
    /// Запрос на получение автомобиля
    /// </summary>
    /// <param name="Id">Идентификатор</param>
    public record GetCarByIdQuery(int Id) : IRequest<GetCarDTO>;
}
