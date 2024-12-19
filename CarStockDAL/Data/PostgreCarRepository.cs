using CarStockDAL.Data.Repos;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;
namespace CarStockDAL.Data
{
    public class PostgreCarRepository<T> : ICarRepository<Car> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Car> _cars;

        public PostgreCarRepository(AppDbContext context) 
        {
            _dbContext = context;
            _cars = context.Cars;

        }


        public async Task CreateCarAsync(Car car)
        {
            _cars.Add(car);
            await SaveAsync();
        }

        public async Task UpdateCarAsync(Car car)
        {
            _cars.Update(car);
            await SaveAsync();
        }

        public async Task DeleteCarAsync(int id)
        {
            // Поиск автомобиля по идентификатору
            var exists = await _cars.Where(c => c.Id == id).AnyAsync();
            if (!exists)
            {
                throw new InvalidOperationException("Car not found.");
            }

            var carToDelete = await _cars.SingleOrDefaultAsync(c => c.Id == id);

            // Удаление автомобиля
            _cars.Remove(carToDelete);

            // Сохранение изменений в базе данных
            await _dbContext.SaveChangesAsync();
        }


        // тут мейби надо править под линкью
        public async Task<Car> GetCarByIdAsync(int id)
        {
            var car = await _cars
                .Include(c => c.Brand)     // Загрузить связанный объект Brand
                .Include(c => c.CarModel)  // Загрузить связанный объект CarModel
                .Include(c => c.Color)     // Загрузить связанный объект Color
                .FirstOrDefaultAsync(c => c.Id == id);  // Фильтруем по ID
            return car;
        }

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


        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
