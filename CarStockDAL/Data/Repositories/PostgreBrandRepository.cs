using System.Drawing;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarStockDAL.Data.Repositories
{
    public class PostgreBrandRepository<T> : PostgreBaseRepository, IBrandRepository<Brand> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Brand> _brands;

        public PostgreBrandRepository(AppDbContext dbContext) : base(dbContext)
        {
            _brands = dbContext.Brands;
        }

        public async Task CreareBrandAsync(Brand brand)
        {
            _brands.Add(brand);
            await SaveAsync();
        }

        public async Task DeleteBrandAsync(int id)
        {
            var brandToDelete = await _brands.FindAsync(id);

            if (brandToDelete != null)
            {
                _brands.Remove(brandToDelete);
                await SaveAsync();
            }
        }

        public async Task<List<Brand>> GetAllBrandsAsync(bool tracked = true)
        {
            IQueryable<Brand> query = _brands;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            return await _brands.FindAsync(id);
        }

        public async Task UpdateBrandAsync(Brand brand)
        {
            _brands.Update(brand);
            await SaveAsync();
        }

        public async Task<Brand?> GetBrandByNameAsync(string name)
        {
            return await _brands.FirstOrDefaultAsync(b => b.Name == name);
        }
    }
}
