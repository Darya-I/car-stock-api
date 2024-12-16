using System.Reflection.Metadata.Ecma335;
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

            var carToDelete = await _cars.FindAsync(id);

            if (carToDelete != null)
            {
                _cars.Remove(carToDelete);
                await SaveAsync();
            }
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _cars.FindAsync(id);
        }

        public async Task<List<Car>> GetAllCarsAsync(bool tracked = true)
        {
            IQueryable<Car> query = _cars;
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
