using CarStockBLL.Interfaces;
using CarStockBLL.Models;
using CarStockDAL.Data;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;

namespace CarStockBLL.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository<Brand> _brandRepository;
        public BrandService(IBrandRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }
        public async Task<Brand> GetBrandByNameAsync(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Brand name cannot be null or empty.");
            }

            var brand = await _brandRepository.GetBrandByNameAsync(name);

            if (brand == null)
            {
                throw new KeyNotFoundException($"Brand with name '{name}' not found.");
            }

            return (new Brand { Name = brand.Name, Id = brand.Id });
        }
    }
}
