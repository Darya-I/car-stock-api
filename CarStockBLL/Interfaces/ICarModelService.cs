using CarStockBLL.Models;
using CarStockDAL.Models;

namespace CarStockBLL.Interfaces
{
    public interface ICarModelService
    {
        Task<OperationResult<CarModel>> GetCarModelByNameAsync(string? name);
    }
}
