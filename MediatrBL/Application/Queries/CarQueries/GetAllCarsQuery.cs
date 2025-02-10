using CarStockBLL.DTO.Car;
using MediatR;

namespace MediatrBL.Application.Queries.CarQueries
{
    /// <summary>
    /// Запрос на получение коллекции автомобилей
    /// </summary>
    public record GetAllCarsQuery() : IRequest<List<GetCarDTO>>;
}
