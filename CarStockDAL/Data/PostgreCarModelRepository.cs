using System.Drawing;
using CarStockDAL.Data.Repos;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarStockDAL.Data
{
    public class PostgreCarModelRepository<T> : ICarModelRepository<CarModel> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<CarModel> _carModels;

        public PostgreCarModelRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync(); // DRY нарушен потому что мы не юзаем юнит оф ворк
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
