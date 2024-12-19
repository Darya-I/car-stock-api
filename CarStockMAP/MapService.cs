using CarStockBLL.Interfaces;
using CarStockBLL.Models;
using CarStockDAL.Models;
using CarStockMAP.DTO;
using CarStockMAP.Mapping;
using CarStockMAP.ViewModels;


namespace CarStockMAP
{
    public class MapService
    {
        public readonly ICarService _carService;

        public MapService(ICarService carService)
        {
            _carService = carService;
        }

        public async Task<IEnumerable<CarDTO>> GetMappedCarsAsync()
        {
            var cars = await _carService.GetAllCarsAsync();

            var mapper = new CarMapper();
            //преобразуем каждый объект Car из коллекции cars в объекты CarDto и создаем список
            var carDtos = cars.Select(car => mapper.MapToCarDto(car)).ToList();  

            return carDtos;

        }


        public async Task<OperationResult<string>> CreateMappedCarAsync(CarDTO carDto)
        {
            try
            {
                var mapper = new CarMapper();

                // преобразуем DTO в модель уровня BLL
                var car = mapper.MapToCar(carDto);

                // передаем преобразованную модель в _carService для создания
                var result = await _carService.CreateCarAsync(car);

                return result;
            }
            catch (Exception ex)
            {
                return OperationResult<string>.Failure($"An error occurred while creating the mapped car: {ex.Message}");
            }
        }


    }
}
