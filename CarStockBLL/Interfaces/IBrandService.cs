using CarStockBLL.Models;
using CarStockDAL.Models;

namespace CarStockBLL.Interfaces
{
    public interface IBrandService
    {
        Task<OperationResult<Brand>> GetBrandByNameAsync(string? name);
    }
}
