using CarStockBLL.CustomException;
using CarStockBLL.DTO.Car;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Queries.CarQueries;

namespace MediatrBL.Application.Handlers.Cars
{
    /// <summary>
    /// Обработчик запроса на получение автомобиля по идентификатору
    /// </summary>
    public class GetCarByIdQueryHandler : IRequestHandler<GetCarByIdQuery, GetCarDTO>
    {
        /// <summary>
        /// Экземпляр репозитория для работы с автомобилями
        /// </summary>
        private readonly ICarRepository<Car> _carRepository;

        public GetCarByIdQueryHandler(ICarRepository<Car> carRepository)
        {
            _carRepository = carRepository;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос на получение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Автомобиль</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<GetCarDTO> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
        {
            var car = await _carRepository.GetCarByIdAsync(request.Id);

            if (car == null)
            {
                throw new EntityNotFoundException();
            }

            return new GetCarDTO
            {
                Id = car.Id,
                Brand = car.Brand.Name,
                CarModel = car.CarModel.Name,
                Color = car.Color.Name,
                Amount = car.Amount,
                IsAvailable = car.IsAvailable
            };
        }
    }
}
