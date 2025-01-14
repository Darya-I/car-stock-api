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
        public async Task<OperationResult<Brand>> GetBrandByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return OperationResult<Brand>.Failure("Brand name cannot be null or empty.");
            }

            var brand = await _brandRepository.GetBrandByNameAsync(name);

            if (brand == null)
            {
                return OperationResult<Brand>.Failure($"Brand with name '{name}' not found.");
            }

            return OperationResult<Brand>.SuccessResult(new Brand { Name = brand.Name, Id = brand.Id });
        }
    }
}
