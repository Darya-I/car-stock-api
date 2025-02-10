using MediatR;

namespace MediatrBL.Application.Queries.CarQueries
{
    /// <summary>
    /// Запрос на проверку существования автомобиля
    /// </summary>
    /// <param name="BrandId">Идентификатор марки</param>
    /// <param name="CarModelId">Идентификатор модели</param>
    /// <param name="ColorId">Идентификатор цвета</param>
    public record CarExistQuery(int BrandId, 
                               int CarModelId,
                               int ColorId)
        : IRequest<bool>;
}
