using CarStockBLL.CustomException;
using CarStockBLL.DTO.Car;
using CarStockDAL.Data;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Data.Repositories;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Commands.CarCommands;

namespace MediatrBL.Application.Handlers.Cars
{
    /// <summary>
    /// Обработчик запроса на обновление автомобиля 
    /// </summary>
    public class UpdateCarCommandHandler : PostgreBaseRepository, IRequestHandler<UpdateCarCommand, CarDTO>
    {
        /// <summary>
        /// Экземпляр репозитория для работы с автомобилями
        /// </summary>
        private readonly ICarRepository<Car> _carRepository;

        public UpdateCarCommandHandler(AppDbContext context,
                                       ICarRepository<Car> carRepository) 
                                       : base(context) 
        {
            _carRepository = carRepository;
        }

        /// <summary>
        /// Обрабатывает запрос 
        /// </summary>
        /// <param name="request">Запрос на обновление</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>DTO автомобиля</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<CarDTO> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
        {
            var existingCar = await _carRepository.GetCarByIdAsync(request.Car.Id);

            if (existingCar == null)
            {
                throw new EntityNotFoundException();
            }

            existingCar.BrandId = request.Car.BrandId;
            existingCar.CarModelId = request.Car.CarModelId;
            existingCar.ColorId = request.Car.ColorId;
            existingCar.Amount = request.Car.Amount;
            existingCar.IsAvailable = request.Car.IsAvailable;

            await _carRepository.UpdateCarAsync(existingCar);

            await SaveAsync();

            return new CarDTO()
            {
                BrandId = existingCar.BrandId,
                CarModelId = existingCar.CarModelId,
                ColorId = existingCar.ColorId,
                Amount = existingCar.Amount,
                IsAvailable = existingCar.IsAvailable
            };
        }
    }
}