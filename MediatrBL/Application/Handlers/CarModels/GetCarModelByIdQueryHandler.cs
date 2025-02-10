using CarStockBLL.CustomException;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Queries.CarModelQueries;

namespace MediatrBL.Application.Handlers.CarModels
{
    /// <summary>
    /// Обработчик запроса на получение модели автомобиля 
    /// </summary>
    public class GetCarModelByIdQueryHandler : IRequestHandler<GetCarModelByIdQuery, CarModel>
    {
        /// <summary>
        /// Экземпляр репозитория для работы с моделями автомобиля
        /// </summary>
        private readonly ICarModelRepository<CarModel> _carModelRepository;

        public GetCarModelByIdQueryHandler(ICarModelRepository<CarModel> carModelRepository)
        {
            _carModelRepository = carModelRepository;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос на получение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Модель</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<CarModel> Handle(GetCarModelByIdQuery request, CancellationToken cancellationToken)
        {
            var carModel = await _carModelRepository.GetCarModelByIdAsync(request.Id);

            if (carModel == null)
            {
                throw new EntityNotFoundException();
            }

            return carModel;
        }
    }
}