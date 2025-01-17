using System.Drawing;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarStockDAL.Data.Repositories
{
    /// <summary>
    /// Репозиторий для операций с моделями автомобилей, реализующий интерфейс <see cref="ICarModelRepository{T}"/>
    /// </summary>
    /// <typeparam name="T">Тип сущности, с которой работает репозиторий</typeparam>
    public class PostgreCarModelRepository<T> : PostgreBaseRepository, ICarModelRepository<CarModel> where T : class
    {
        /// <summary>
        /// Коллекция сущностей моделей
        /// </summary>
        private readonly DbSet<CarModel> _carModels;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="PostgreCarModelRepository{T}"/>.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных</param>
        public PostgreCarModelRepository(AppDbContext dbContext) : base(dbContext)
        {
            _carModels = dbContext.Models;
        }

        /// <summary>
        /// Создает новую запись модели автомобиля
        /// </summary>
        /// <param name="carModel">Объект модели автомобиля для создания</param>
        public async Task CreateCarModelAsync(CarModel carModel)
        {
            _carModels.Add(carModel);
            await SaveAsync();
        }

        /// <summary>
        /// Удаляет запись модели автомобиля по Id
        /// </summary>
        /// <param name="id">Id модели</param>
        public async Task DeleteCarModelAsync(int id)
        {
            var carModelToDelete = await _carModels.FindAsync(id);

            if (carModelToDelete != null)
            {
                _carModels.Remove(carModelToDelete);
                await SaveAsync();
            }
        }

        /// <summary>
        /// Получает запись модели автомобиля по Id
        /// </summary>
        /// <param name="id">Id модели</param>
        /// <returns>Марка автомобиля</returns>
        public async Task<CarModel> GetCarModelByIdAsync(int id)
        {
            return await _carModels.FindAsync(id);
        }

        /// <summary>
        /// Получает список всех моделей автомобилей
        /// </summary>
        /// <param name="tracked">Указывает, отслеживаются ли изменения сущностей. По умолчанию true</param>
        /// <returns>Список моделей</returns>
        public async Task<List<CarModel>> GetCarModelsAsync(bool tracked = true)
        {
            IQueryable<CarModel> query = _carModels;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Обновляет запись модели автомобиля
        /// </summary>
        /// <param name="carModel">Объект модели автомобиля для обновления</param>
        public async Task UpdateCarModelAsync(CarModel carModel)
        {
            _carModels.Update(carModel);
            await SaveAsync();
        }


        /// <summary>
        /// Получает модель автомобиля по названию
        /// </summary>
        /// <param name="name">Название модели</param>
        /// <returns>Модель автомобиля</returns>
        public async Task<CarModel?> GetCarModelByNameAsync(string name)
        {
            return await _carModels.FirstOrDefaultAsync(b => b.Name == name);
        }
    }
}
