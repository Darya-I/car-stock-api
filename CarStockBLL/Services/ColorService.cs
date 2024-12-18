using CarStockBLL.Interfaces;
using CarStockBLL.Models;
using CarStockDAL.Data.Repos;
using CarStockDAL.Models;

namespace CarStockBLL.Services
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository<Color> _colorRepository;

        public ColorService(IColorRepository<Color> colorRepository)
        {
            _colorRepository = colorRepository;
        }

        public async Task<OperationResult<Color>> GetColorByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return OperationResult<Color>.Failure("Color name cannot be null or empty.");
            }

            var color = await _colorRepository.GetColorByNameAsync(name);
            if (color == null)
            {
                return OperationResult<Color>.Failure($"Color with name '{name}' not found.");
            }

            return OperationResult<Color>.SuccessResult(new Color { Name = color.Name, Id = color.Id });
        }

    }


}
