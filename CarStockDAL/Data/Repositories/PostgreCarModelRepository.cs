using System.Drawing;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarStockDAL.Data.Repositories
{
    /// <summary>
    /// Репозиторий для операций с моделями автомобилей, реализующий интерфейс <see cref="ICarModelRepository{T}"/>.
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

        public async Task CreateCarModelAsync(CarModel carModel)
        {
            _carModels.Add(carModel);
            await SaveAsync();
        }

        public async Task DeleteCarModelAsync(int id)
        {
            var carModelToDelete = await _carModels.FindAsync(id);

            if (carModelToDelete != null)
            {
                _carModels.Remove(carModelToDelete);
                await SaveAsync();
            }
        }

        public async Task<CarModel> GetCarModelByIdAsync(int id)
        {
            return await _carModels.FindAsync(id);
        }

        public async Task<List<CarModel>> GetCarModelsAsync(bool tracked = true)
        {
            IQueryable<CarModel> query = _carModels;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task UpdateCarModelAsync(CarModel carModel)
        {
            _carModels.Update(carModel);
            await SaveAsync();
        }

        public async Task<CarModel?> GetCarModelByNameAsync(string name)
        {
            return await _carModels.FirstOrDefaultAsync(b => b.Name == name);
        }
    }
}
