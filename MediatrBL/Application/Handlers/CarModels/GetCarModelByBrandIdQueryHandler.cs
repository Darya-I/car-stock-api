using CarStockBLL.CustomException;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Queries.CarModelQueries;

namespace MediatrBL.Application.Handlers.CarModels
{
    public class GetCarModelByBrandIdQueryHandler : IRequestHandler<GetCarModelByBrandIdQuery, List<CarModel>>
    {
        /// <summary>
        /// Экземпляр репозитория для работы с моделями автомобиля
        /// </summary>
        private readonly ICarModelRepository<CarModel> _carModelRepository;

        public GetCarModelByBrandIdQueryHandler(ICarModelRepository<CarModel> carModelRepository)
        {
            _carModelRepository = carModelRepository;
        }

        /// <summary>
        /// Обрабатывает команду на получение списка моделей для определенной марки
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<List<CarModel>> Handle(GetCarModelByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var carModels = await _carModelRepository.GetModelByBrandIdAsync(request.id);
            
            if (carModels == null)
            {
                throw new EntityNotFoundException();
            }

            return carModels;
        }
    }
}