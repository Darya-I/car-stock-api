using CarStockBLL.Models;
using CarStockDAL.Models;

namespace CarStockBLL.Interfaces
{
    public interface IColorService
    {
        Task<OperationResult<Color>> GetColorByNameAsync(string? name);   
    }
}
