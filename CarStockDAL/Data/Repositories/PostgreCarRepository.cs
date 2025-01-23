using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;
namespace CarStockDAL.Data.Repositories
{
    /// <summary>
    /// Репозиторий для операций с автомобилями, реализующий интерфейс <see cref="ICarRepository{T}"/>
    /// </summary>
    /// <typeparam name="T">Тип сущности, с которой работает репозиторий</typeparam>
    public class PostgreCarRepository<T> : PostgreBaseRepository, ICarRepository<Car> where T : class
    {
        /// <summary>
        /// Коллекция сущностей автомобилей
        /// </summary>
        private readonly DbSet<Car> _cars;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="PostgreCarRepository{T}"/>.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных</param>
        public PostgreCarRepository(AppDbContext dbContext) : base(dbContext)
        {
            _cars = dbContext.Cars;
        }

        /// <summary>
        /// Создает новую запись автомобиля
        /// </summary>
        /// <param name="car">Объект автомобиля для создания</param>
        public async Task CreateCarAsync(Car car)
        {
            _cars.Add(car);
            await SaveAsync();
        }

        /// <summary>
        /// Обновляет запись автомобиля
        /// </summary>
        /// <param name="car">Объект автомобиля для обновления</param>
        public async Task UpdateCarAsync(Car car)
        {
            _cars.Update(car);
            await SaveAsync();
        }

        /// <summary>
        /// Удаляет запись автомобиля по Id
        /// </summary>
        /// <param name="id">Id автомобиля</param>
        public async Task DeleteCarAsync(int id)
        {
            var carToDelete = await _cars.SingleOrDefaultAsync(c => c.Id == id);
            _cars.Remove(carToDelete);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Получает запись автомобиля по Id
        /// </summary>
        /// <param name="id">Id автомобиля</param>
        /// <returns>Автомобиль</returns>
        public async Task<Car> GetCarByIdAsync(int id)
        {
            var car = await _cars
                .Include(c => c.Brand)     // Загрузить связанный объект Brand
                .Include(c => c.CarModel)  // Загрузить связанный объект CarModel
                .Include(c => c.Color)     // Загрузить связанный объект Color
                .FirstOrDefaultAsync(c => c.Id == id);  // Фильтруем по ID

            return car;
        }

        /// <summary>
        /// Получает список всех автомобилей
        /// </summary>
        /// <param name="tracked">Указывает, отслеживаются ли изменения сущностей. По умолчанию true</param>
        /// <returns>Список автомобилей</returns>
        public async Task<List<Car>> GetAllCarsAsync(bool tracked = true)
        {
            IQueryable<Car> query = _cars
                .Include(c => c.Brand)
                .Include(c => c.CarModel)
                .Include(c => c.Color);

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<bool> CarExistAsync(int brandId, int carModelId, int colorId)
        {
            return await _cars.AnyAsync(c =>
            c.BrandId == brandId &&
            c.CarModelId == carModelId &&
            c.ColorId == colorId);
        }
    }
}