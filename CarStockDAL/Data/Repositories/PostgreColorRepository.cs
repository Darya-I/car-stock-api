using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarStockDAL.Data.Repositories
{
    /// <summary>
    /// Репозиторий для операций с марками автомобилей, реализующий интерфейс <see cref="IColorRepository{T}"/>
    /// </summary>
    /// <typeparam name="T">Тип сущности, с которой работает репозиторий</typeparam>
    public class PostgreColorRepository<T> : PostgreBaseRepository, IColorRepository<Color> where T : class
    {
        /// <summary>
        /// Коллекция сущностей цветов
        /// </summary>
        private readonly DbSet<Color> _colors;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="PostgreColorRepository{T}"/>.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных</param>
        public PostgreColorRepository(AppDbContext dbContext) : base(dbContext)
        {
            _colors = dbContext.Colors;
        }

        /// <summary>
        /// Создает новую запись цвета автомобиля
        /// </summary>
        /// <param name="color">Объект цвета автомобиля для создания</param>
        public async Task CreateColorAsync(Color color)
        {
            _colors.Add(color);
            await SaveAsync();
        }

        /// <summary>
        /// Удаляет запись цвета автомобиля по Id
        /// </summary>
        /// <param name="id">Id цвета</param>
        public async Task DeleteColorAsync(int id)
        {
            var colorToDelete = await _colors.FindAsync(id);

            if (colorToDelete != null)
            {
                _colors.Remove(colorToDelete);
                await SaveAsync();
            }
        }

        /// <summary>
        /// Получает список всех цветов автомобилей
        /// </summary>
        /// <param name="tracked">Указывает, отслеживаются ли изменения сущностей. По умолчанию true</param>
        /// <returns>Список цветов</returns>
        public async Task<List<Color>> GetAllColorsAsync(bool tracked = true)
        {
            IQueryable<Color> query = _colors;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Получает запись цвета автомобиля по Id
        /// </summary>
        /// <param name="id">Id цвета</param>
        /// <returns>Цвет автомобиля</returns>
        public async Task<Color> GetColorByIdAsync(int id)
        {
            return await _colors.FindAsync(id);
        }

        /// <summary>
        /// Обновляет запись цвета автомобиля
        /// </summary>
        /// <param name="color">Объект цвета автомобиля для обновления</param>
        public async Task UpdateColorAsync(Color color)
        {
            _colors.Update(color);
            await SaveAsync();
        }

        /// <summary>
        /// Получает цвет автомобиля по названию
        /// </summary>
        /// <param name="name">Название цвета</param>
        /// <returns>Цвет автомобиля</returns>
        public async Task<Color?> GetColorByNameAsync(string name)
        {
            return await _colors.FirstOrDefaultAsync(b => b.Name == name);
        }
    }
}