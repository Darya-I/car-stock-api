using CarStockBLL.CustomException;
using CarStockBLL.DTO.Car;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Queries.CarQueries;

namespace MediatrBL.Application.Handlers.Cars
{
    /// <summary>
    /// Обработчик запроса на получение списка автомобилей
    /// </summary>
    public class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, List<GetCarDTO>>
    {
        /// <summary>
        /// Экземпляр репозитория для работы с автомобилями
        /// </summary>
        private readonly ICarRepository<Car> _carRepository;

        public GetAllCarsQueryHandler(ICarRepository<Car> carRepository)
        {
            _carRepository = carRepository;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос на получение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список автомобилей</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<List<GetCarDTO>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
        {
            var cars = await _carRepository.GetAllCarsAsync();

            if (cars == null)
            {
                throw new EntityNotFoundException();
            }

            return cars.Select(car => new GetCarDTO
            {
                Id = car.Id,
                Brand = car.Brand.Name,
                CarModel = car.CarModel.Name,
                Color = car.Color.Name,
                Amount = car.Amount,
                IsAvailable = car.IsAvailable
            }).ToList();
        }
    }
}