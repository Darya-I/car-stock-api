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

        public async Task<OperationResult<CarModel>> GetCarModelByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return OperationResult<CarModel>.Failure("Car model name cannot be null or empty.");
            }

            var carModel = await _carModelRepository.GetCarModelByNameAsync(name);
            if (carModel == null)
            {
                return OperationResult<CarModel>.Failure($"Car model with name '{name}' not found.");
            }

            return OperationResult<CarModel>.SuccessResult(new CarModel { Name = carModel.Name, Id = carModel.Id });
        }
    }
}
