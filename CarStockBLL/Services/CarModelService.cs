using CarStockBLL.Interfaces;
using CarStockBLL.Models;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;

namespace CarStockBLL.Services
{
    public class CarModelService : ICarModelService
    {
        private ICarModelRepository<CarModel> _carModelRepository;

        public CarModelService(ICarModelRepository<CarModel> carModelRepository)
        {
            _carModelRepository = carModelRepository;
        }

        public async Task<CarModel> GetCarModelByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Car model name cannot be null or empty.");
            }

            var carModel = await _carModelRepository.GetCarModelByNameAsync(name);
            if (carModel == null)
            {
                throw new KeyNotFoundException($"Car model with name '{name}' not found.");
            }

            return (new CarModel { Name = carModel.Name, Id = carModel.Id });
        }
    }
}
