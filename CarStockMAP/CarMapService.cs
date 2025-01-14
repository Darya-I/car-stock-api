using CarStockBLL.Interfaces;
using CarStockBLL.Models;
using CarStockBLL.Services;
using CarStockDAL.Models;
using CarStockMAP.DTO.Car;
using CarStockMAP.Mapping;

namespace CarStockMAP
{
    /// <summary>
    /// Сервис для маппинга автомобилей
    /// </summary>
    public class CarMapService
    {
        /// <summary>
        /// Сервис операций над автомобилями
        /// </summary>
        private readonly ICarService _carService;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CarMapService"/>.
        /// </summary>
        /// <param name="carService">Сервис операций над автомобилями</param>
        public CarMapService(ICarService carService)
        {
            _carService = carService;
        }

        /// <summary>
        /// Маппит DTO обновленного автомобиля на Car, передает в CarService
        /// </summary>
        /// <param name="carUpdateDTO">Обновленный автомобиль</param>
        public async Task GetUpdatedMappedCarAsync(CarUpdateDTO carUpdateDTO)
        {
            var mapper = new CarMapper();
            var updatedCar = mapper.MapUpdateCarDtoToCar(carUpdateDTO);

            await _carService.UpdateCarAsync(updatedCar);
        }

        /// <summary>
        /// Получает список автомобилей из CarService, маппит каждый элемент на DTO автомобиля
        /// </summary>
        /// <returns>DTO списка автомобилей</returns>
        public async Task<IEnumerable<CarDTO>> GetMappedCarsAsync()
        {
            var cars = await _carService.GetAllCarsAsync();

            var mapper = new CarMapper();
            
            //преобразуем каждый объект Car из коллекции cars в объекты CarDto и создаем список
            var carDtos = cars.Select(car => mapper.MapCarToCarDto(car)).ToList();

            return carDtos;

        }

        /// <summary>
        /// Маппит DTO автомобиля на Car, передает CarService
        /// </summary>
        /// <param name="carDto">DTO автомобиля</param>
        /// <returns></returns>
        /// <exception cref="OperationResult"></exception>
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
