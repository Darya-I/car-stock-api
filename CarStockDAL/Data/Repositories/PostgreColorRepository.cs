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

        public async Task CreateColorAsync(Color color)
        {
            _colors.Add(color);
            await SaveAsync();
        }

        public async Task DeleteColorAsync(int id)
        {
            var colorToDelete = await _colors.FindAsync(id);

            if (colorToDelete != null)
            {
                _colors.Remove(colorToDelete);
                await SaveAsync();
            }
        }

        public async Task<List<Color>> GetAllColorsAsync(bool tracked = true)
        {
            IQueryable<Color> query = _colors;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<Color> GetColorByIdAsync(int id)
        {
            return await _colors.FindAsync(id);
        }

        public async Task UpdateColorAsync(Color color)
        {
            _colors.Update(color);
            await SaveAsync();
        }

        public async Task<Color?> GetColorByNameAsync(string name)
        {
            return await _colors.FirstOrDefaultAsync(b => b.Name == name);
        }
    }
}
