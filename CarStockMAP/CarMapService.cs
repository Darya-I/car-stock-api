using CarStockBLL.Interfaces;
using CarStockBLL.Models;
using CarStockBLL.Services;
using CarStockDAL.Models;
using CarStockMAP.DTO.Car;
using CarStockMAP.Mapping;

namespace CarStockMAP
{
    public class CarMapService
    {
        private readonly ICarService _carService;

        public CarMapService(ICarService carService)
        {
            _carService = carService;
        }

        public async Task GetUpdatedMappedCarAsync(CarUpdateDTO carUpdateDTO)
        {
            var mapper = new CarMapper();
            var updatedCar = mapper.MapUpdateCarDtoToCar(carUpdateDTO);

            var response = await _carService.UpdateCarAsync(updatedCar);
        }

        public async Task<IEnumerable<CarDTO>> GetMappedCarsAsync()
        {
            var cars = await _carService.GetAllCarsAsync();

            var mapper = new CarMapper();
            //преобразуем каждый объект Car из коллекции cars в объекты CarDto и создаем список
            var carDtos = cars.Select(car => mapper.MapCarToCarDto(car)).ToList();

            return carDtos;

        }
        public async Task<OperationResult<string>> CreateMappedCarAsync(CarDTO carDto)
        {
            try
            {
                var mapper = new CarMapper();

                // преобразуем DTO в модель уровня BLL
                var car = mapper.MapCarDTOToCar(carDto);

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
