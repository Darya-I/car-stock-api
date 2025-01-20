using CarStockBLL.CustomException;
using CarStockBLL.Interfaces;
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
        public async Task<CarUpdateDTO> GetUpdatedMappedCarAsync(CarUpdateDTO carUpdateDTO)
        {
            try
            {
                if (carUpdateDTO == null) 
                {
                    throw new ValidationErrorException("Invalid data");
                }
                var mapper = new CarMapper();
                var updatedCar = mapper.MapUpdateCarDtoToCar(carUpdateDTO);
                await _carService.UpdateCarAsync(updatedCar);
                return carUpdateDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Получает список автомобилей из CarService, маппит каждый элемент на DTO автомобиля
        /// </summary>
        /// <returns>DTO списка автомобилей</returns>
        public async Task<IEnumerable<CarDTO>> GetMappedCarsAsync()
        {
            try
            {
                var cars = await _carService.GetAllCarsAsync();
                var mapper = new CarMapper();
                //Преобразуем каждый объект Car из коллекции cars в объекты CarDto и создаем список
                var carDtos = cars.Select(car => mapper.MapCarToCarDto(car)).ToList();
                return carDtos;
            }
            catch (Exception) 
            {
                throw;
            }
        }

        /// <summary>
        /// Маппит DTO автомобиля на Car, передает CarService
        /// </summary>
        /// <param name="carDto">DTO автомобиля</param>
        /// <returns>Строка с результатом</returns>
        public async Task<string> CreateMappedCarAsync(CarDTO carDto)
        {
            try
            {
                if (carDto == null)
                {
                    throw new ValidationErrorException("Invalid data");
                }
                var mapper = new CarMapper();
                // Преобразуем DTO в модель уровня BLL
                var car = mapper.MapCarDTOToCar(carDto);
                // Передаем преобразованную модель в _carService для создания
                var result = await _carService.CreateCarAsync(car);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}