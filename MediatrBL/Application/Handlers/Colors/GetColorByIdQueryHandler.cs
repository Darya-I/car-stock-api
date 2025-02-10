using CarStockBLL.CustomException;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Queries.ColorQueries;

namespace MediatrBL.Application.Handlers.Colors
{
    /// <summary>
    /// Обработчик запроса на получение цвета автомобиля 
    /// </summary>
    public class GetColorByIdQueryHandler : IRequestHandler<GetColorByIdQuery, Color>
    {
        /// <summary>
        /// Экземпляр репозитория для работы с цветами автомобиля
        /// </summary>
        private readonly IColorRepository<Color> _colorRepository;

        public GetColorByIdQueryHandler(IColorRepository<Color> colorRepository)
        {
            _colorRepository = colorRepository;
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос на получение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Цвет автомобиля</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<Color> Handle(GetColorByIdQuery request, CancellationToken cancellationToken)
        {
            var color = await _colorRepository.GetColorByIdAsync(request.Id);

            if (color == null)
            {
                throw new EntityNotFoundException();
            }

            return color;
        }
    }
}
