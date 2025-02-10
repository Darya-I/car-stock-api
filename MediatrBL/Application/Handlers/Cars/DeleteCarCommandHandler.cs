using CarStockBLL.CustomException;
using CarStockDAL.Data;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Data.Repositories;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Commands.CarCommands;

namespace MediatrBL.Application.Handlers.Cars
{
    /// <summary>
    /// Обработчик запроса на удаление автомобиля 
    /// </summary>
    public class DeleteCarCommandHandler : PostgreBaseRepository, IRequestHandler<DeleteCarCommand>
    {
        /// <summary>
        /// Экземпляр репозитория для работы с автомобилями
        /// </summary>
        private readonly ICarRepository<Car> _carRepository;

        public DeleteCarCommandHandler(AppDbContext context,
                                                    ICarRepository<Car> carRepository)
                                                    : base(context)
        {
            _carRepository = carRepository;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос на удаление</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task Handle(DeleteCarCommand request, CancellationToken cancellationToken)
        {
            var carToDelete = await _carRepository.GetCarByIdAsync(request.Id);

            if (carToDelete == null)
            {
                throw new EntityNotFoundException();
            }

            await _carRepository.DeleteCarAsync(request.Id);

            await SaveAsync();
        }
    }
}