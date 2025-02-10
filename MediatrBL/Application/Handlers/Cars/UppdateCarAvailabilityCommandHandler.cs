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
    /// Обработчик запроса на обновление доступности автомобиля 
    /// </summary>
    public class UppdateCarAvailabilityCommandHandler : PostgreBaseRepository, IRequestHandler<UpdateCarAvailabilityCommand, CarAvailabilityDTO>
    {
        /// <summary>
        /// Экземпляр репозитория для работы с автомобилями
        /// </summary>
        private readonly ICarRepository<Car> _carRepository;

        public UppdateCarAvailabilityCommandHandler(AppDbContext context,
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
        /// <returns>DTO обновления доступности автомобиля</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<CarAvailabilityDTO> Handle(UpdateCarAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var existingCar = await _carRepository.GetCarByIdAsync(request.Id);

            if (existingCar == null)
            {
                throw new EntityNotFoundException();
            }
            existingCar.IsAvailable = request.IsAvailable;

            await _carRepository.UpdateCarAsync(existingCar);

            await SaveAsync();

            return new CarAvailabilityDTO()
            {
                Id = existingCar.Id,
                IsAvailable = existingCar.IsAvailable,
            };
        }
    }
}