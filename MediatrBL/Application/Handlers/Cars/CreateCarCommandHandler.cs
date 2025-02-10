using CarStockBLL.CustomException;
using CarStockBLL.DTO.Car;
using CarStockDAL.Data;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Data.Repositories;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Commands.CarCommands;
using MediatrBL.Application.Queries.BrandQueries;
using MediatrBL.Application.Queries.CarModelQueries;
using MediatrBL.Application.Queries.CarQueries;
using MediatrBL.Application.Queries.ColorQueries;



namespace MediatrBL.Application.Handlers.Cars
{
    /// <summary>
    /// Обработчик запроса на создание автомобиля 
    /// </summary>
    public class CreateCarCommandHandler : PostgreBaseRepository, IRequestHandler<CreateCarCommand, CarDTO>
    {
        /// <summary>
        /// Экземпляр MediatR
        /// </summary>
        private IMediator _mediator;

        /// <summary>
        /// Экземпляр репозитория для работы с автомобилями
        /// </summary>
        private readonly ICarRepository<Car> _carRepository;

        public CreateCarCommandHandler(AppDbContext context,
                                       ICarRepository<Car> carRepository,
                                       IMediator mediator)
                                       : base(context)
        {
            _carRepository = carRepository;
            _mediator = mediator;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос на создание</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>DTO автомобиля</returns>
        /// <exception cref="EntityAlreadyExistsException"></exception>
        public async Task<CarDTO> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            var brand = await _mediator.Send(new GetBrandByIdQuery(request.Car.BrandId), cancellationToken);
            var carModel = await _mediator.Send(new GetCarModelByIdQuery(request.Car.CarModelId), cancellationToken);
            var color = await _mediator.Send(new GetColorByIdQuery(request.Car.ColorId), cancellationToken);

            bool carExist = await _mediator.Send(new CarExistQuery(brand.Id, carModel.Id, color.Id), cancellationToken);

            if (carExist)
            {
                throw new EntityAlreadyExistsException();
            }

            var newCar = new Car()
            {
                BrandId = brand.Id,
                ColorId = color.Id,
                CarModelId = carModel.Id,
                Amount = request.Car.Amount,
                IsAvailable = request.Car.IsAvailable
            };

            await _carRepository.CreateCarAsync(newCar);
            await SaveAsync();

            return new CarDTO
            {
                Id = newCar.Id,
                BrandId = newCar.BrandId,
                CarModelId = newCar.CarModelId,
                ColorId = newCar.ColorId,
                Amount = newCar.Amount,
                IsAvailable = newCar.IsAvailable
            };
        }
    }
}