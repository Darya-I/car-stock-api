using CarStockDAL.Data.Repos;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarStockDAL.Data
{
    public class PostgreColorRepository<T> : IColorRepository<Color> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Color> _colors;

        public PostgreColorRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync(); // DRY нарушен потому что мы не юзаем юнит оф ворк
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
