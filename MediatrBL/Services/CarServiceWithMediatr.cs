using CarStockBLL.DTO.Car;
using CarStockBLL.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Commands.CarCommands;
using MediatrBL.Application.Queries.CarQueries;

namespace MediatrBL.Services
{
    /// <summary>
    /// Сервис операций с автомобилями с использованием MediatR
    /// Делегирует выполнение команд и запросов через MediatR
    /// </summary>
    public class CarServiceWithMediatr : ICarService
    {
        /// <summary>
        /// Экземпляр MediatR для отправки команд и запросов.
        /// </summary>
        private readonly IMediator _mediator;

        public CarServiceWithMediatr(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Создает автомобиль
        /// </summary>
        /// <param name="car">Объект автомобиля</param>
        /// <returns>DTO автомобиля</returns>
        public async Task<CarDTO> CreateCarAsync(Car car)
        {
            return await _mediator.Send(new CreateCarCommand(car));
        }

        /// <summary>
        /// Удаляет автомобиль
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        public async Task DeleteCarAsync(int id)
        {
            await _mediator.Send(new DeleteCarCommand(id));
        }

        /// <summary>
        /// Получает список автомобилей
        /// </summary>
        /// <returns>Список DTO получения автомобилей</returns>
        public async Task<List<GetCarDTO>> GetAllCarsAsync()
        {
            return await _mediator.Send(new GetAllCarsQuery());
        }

        /// <summary>
        /// Получает автомобиль по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <returns></returns>
        public async Task<GetCarDTO> GetCarByIdAsync(int id)
        {
            return await _mediator.Send(new GetCarByIdQuery(id));
        }

        /// <summary>
        /// Обновляет количество автомобиля
        /// </summary>
        /// <param name="id">Идентификатор автомобиля</param>
        /// <param name="amount">Количество</param>
        /// <returns>DTO обновления количества</returns>
        public async Task<CarAmountDTO> UpdateCarAmountAsync(int id, int amount)
        {
            return await _mediator.Send(new UpdateCarAmountCommand(id, amount));
        }

        /// <summary>
        /// Обновление автомобиля
        /// </summary>
        /// <param name="car">Объект автомобиля</param>
        /// <returns>DTO автомобиля</returns>
        public async Task<CarDTO> UpdateCarAsync(Car car)
        {
            return await _mediator.Send(new UpdateCarCommand(car));
        }

        /// <summary>
        /// Обновление доступности автомобиля
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="isAvaible">Доступность</param>
        /// <returns>DTO обновления доступности</returns>
        public async Task<CarAvailabilityDTO> UpdateCarAvailabilityAsync(int id, bool isAvaible)
        {
            return await _mediator.Send(new UpdateCarAvailabilityCommand(id, isAvaible));
        }
    }
}
