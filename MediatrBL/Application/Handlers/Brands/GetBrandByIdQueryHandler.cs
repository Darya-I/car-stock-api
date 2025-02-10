using CarStockBLL.CustomException;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Queries.BrandQueries;

namespace MediatrBL.Application.Handlers.Brands
{
    /// <summary>
    /// Обработчик запроса на получение марки автомобиля 
    /// </summary>
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, Brand>
    {
        /// <summary>
        /// Экземпляр репозитория для работы с марками автомобиля
        /// </summary>
        private readonly IBrandRepository<Brand> _brandRepository;

        public GetBrandByIdQueryHandler(IBrandRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос на получение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Марка</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<Brand> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetBrandByIdAsync(request.Id);

            if (brand == null)
            {
                throw new EntityNotFoundException();
            }

            return brand;
        }
    }
}
