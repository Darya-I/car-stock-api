using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Queries.CarQueries;

namespace MediatrBL.Application.Handlers.Cars
{
    /// <summary>
    /// Обработчик запроса на проверку существования автомобиля 
    /// </summary>
    public class CarExistQueryHandler : IRequestHandler<CarExistQuery, bool>
    {
        /// <summary>
        /// Экземпляр репозитория для работы с автомобилями
        /// </summary>
        private readonly ICarRepository<Car> _carRepository;

        public CarExistQueryHandler(ICarRepository<Car> carRepository)
        {
            _carRepository = carRepository;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос на проверку</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns> <c>True</c> Если существует, иначе <c>False</c></returns>
        public async Task<bool> Handle(CarExistQuery request, CancellationToken cancellationToken)
        {
            return await _carRepository.CarExistAsync(request.BrandId, request.CarModelId, request.ColorId);
        }
    }
}