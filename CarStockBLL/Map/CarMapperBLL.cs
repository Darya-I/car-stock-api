using CarStockBLL.DTO.Car;
using CarStockDAL.Models;
using Riok.Mapperly.Abstractions;

namespace CarStockBLL.Map
{
    [Mapper]
    public partial class CarMapperBLL
    {
        /// <summary>
        /// Маппинг с объекта автомобиля на DTO
        /// </summary>
        /// <param name="car">Объект автомобиля</param>
        /// <returns>DTO автомобиля</returns>
        [MapProperty(nameof(car.Id), nameof(CarDTO.Id))]
        [MapProperty(nameof(car.BrandId), nameof(CarDTO.BrandId))]
        [MapProperty(nameof(car.CarModelId), nameof(CarDTO.CarModelId))]
        [MapProperty(nameof(car.ColorId), nameof(CarDTO.ColorId))]
        [MapProperty(nameof(car.Amount), nameof(CarDTO.Amount))]
        [MapProperty(nameof(car.IsAvailable), nameof(CarDTO.IsAvailable))]
        public partial CarDTO CarToCarDto(Car car);

        /// <summary>
        /// Маппинг с объекта автомобиля на DTO c названиями
        /// </summary>
        /// <param name="car">Объект автомобиля</param>
        /// <returns>DTO автомобиля</returns>
        [MapProperty(nameof(car.Id), nameof(GetCarDTO.Id))]
        [MapProperty(nameof(car.Brand.Name), nameof(GetCarDTO.Brand))]
        [MapProperty(nameof(car.CarModel.Name), nameof(GetCarDTO.CarModel))]
        [MapProperty(nameof(car.Color.Name), nameof(GetCarDTO.Color))]
        [MapProperty(nameof(car.Amount), nameof(CarDTO.Amount))]
        [MapProperty(nameof(car.IsAvailable), nameof(CarDTO.IsAvailable))]
        public partial GetCarDTO CarToGetCarDto(Car car);

        /// <summary>
        /// Маппинг с DTO на объект автомобиля 
        /// </summary>
        /// <param name="carDto">DTO автомобиля</param>
        /// <returns>Объект автомобиля</returns>
        [MapProperty(nameof(carDto.Id), nameof(Car.Id))]
        [MapProperty(nameof(carDto.BrandId), nameof(Car.BrandId))]
        [MapProperty(nameof(carDto.CarModelId), nameof(Car.CarModelId))]
        [MapProperty(nameof(carDto.ColorId), nameof(Car.ColorId))]
        [MapProperty(nameof(carDto.Amount), nameof(Car.Amount))]
        [MapProperty(nameof(carDto.IsAvailable), nameof(Car.IsAvailable))]
        public partial Car CarDtoToCar(CarDTO carDto);


        /// <summary>
        /// Маппинг с объекта автомобиля на DTO доступности
        /// </summary>
        /// <param name="car">Объект автомобиля</param>
        /// <returns>DTO доступности</returns>
        [MapProperty(nameof(car.Id), nameof(CarAvailabilityDTO.Id))]
        [MapProperty(nameof(car.IsAvailable), nameof(CarAvailabilityDTO.IsAvailable))]
        public partial CarAvailabilityDTO CarAvailabilityUpdateDTO(Car car);


        /// <summary>
        /// Маппинг с объекта автомобиля на DTO количества
        /// </summary>
        /// <param name="car">Объект автомобиля</param>
        /// <returns>DTO количества</returns>
        [MapProperty(nameof(car.Id), nameof(CarAmountDTO.Id))]
        [MapProperty(nameof(car.Amount), nameof(CarAmountDTO.Amount))]
        public partial CarAmountDTO CarAmountToDto(Car car);
    }
}
