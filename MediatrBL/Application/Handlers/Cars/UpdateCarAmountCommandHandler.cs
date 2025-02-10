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
    /// Обработчик запроса на обновление количества автомобиля
    /// </summary>
    public class UpdateCarAmountCommandHandler : PostgreBaseRepository, IRequestHandler<UpdateCarAmountCommand, CarAmountDTO>
    {
        /// <summary>
        /// Экземпляр репозитория для работы с автомобилями
        /// </summary>
        private readonly ICarRepository<Car> _carRepository;

        public UpdateCarAmountCommandHandler(AppDbContext context,
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
        /// <returns>DTO обновления количества автомобиля</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<CarAmountDTO> Handle(UpdateCarAmountCommand request, CancellationToken cancellationToken)
        {
            var existingCar = await _carRepository.GetCarByIdAsync(request.Id);

            if (existingCar == null)
            {
                throw new EntityNotFoundException();
            }

            existingCar.Amount = request.Amount;

            await _carRepository.UpdateCarAsync(existingCar);

            await SaveAsync();

            return new CarAmountDTO()
            {
                Id = existingCar.Id,
                Amount = existingCar.Amount,
            };
        }
    }
}