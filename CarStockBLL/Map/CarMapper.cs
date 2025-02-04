using CarStockBLL.DTO.Car;
using CarStockDAL.Models;
using Riok.Mapperly.Abstractions;

namespace CarStockBLL.Map
{
    [Mapper]
    public partial class CarMapper
    {
        /// <summary>
        /// Маппинг с объекта автомобиля на DTO
        /// </summary>
        /// <param name="car">Объект автомобиля</param>
        /// <returns>DTO автомобиля</returns>
        [MapProperty(nameof(Car.Id), nameof(CarDTO.Id))]
        [MapProperty(nameof(Car.BrandId), nameof(CarDTO.BrandId))]
        [MapProperty(nameof(Car.CarModelId), nameof(CarDTO.CarModelId))]
        [MapProperty(nameof(Car.ColorId), nameof(CarDTO.ColorId))]
        [MapProperty(nameof(Car.Amount), nameof(CarDTO.Amount))]
        [MapProperty(nameof(Car.IsAvailable), nameof(CarDTO.IsAvailable))]
        public partial CarDTO CarToCarDto(Car car);

        /// <summary>
        /// Маппинг с объекта автомобиля на DTO c названиями
        /// </summary>
        /// <param name="car">Объект автомобиля</param>
        /// <returns>DTO автомобиля</returns>
        [MapProperty(nameof(Car.Id), nameof(GetCarDTO.Id))]
        [MapProperty(nameof(Car.Brand.Name), nameof(GetCarDTO.Brand))]
        [MapProperty(nameof(Car.CarModel.Name), nameof(GetCarDTO.CarModel))]
        [MapProperty(nameof(Car.Color.Name), nameof(GetCarDTO.Color))]
        [MapProperty(nameof(Car.Amount), nameof(CarDTO.Amount))]
        [MapProperty(nameof(Car.IsAvailable), nameof(CarDTO.IsAvailable))]
        public partial GetCarDTO CarToGetCarDto(Car car);

        /// <summary>
        /// Маппинг с DTO на объект автомобиля 
        /// </summary>
        /// <param name="carDto">DTO автомобиля</param>
        /// <returns>Объект автомобиля</returns>
        [MapProperty(nameof(CarDTO.Id), nameof(Car.Id))]
        [MapProperty(nameof(CarDTO.BrandId), nameof(Car.BrandId))]
        [MapProperty(nameof(CarDTO.CarModelId), nameof(Car.CarModelId))]
        [MapProperty(nameof(CarDTO.ColorId), nameof(Car.ColorId))]
        [MapProperty(nameof(CarDTO.Amount), nameof(Car.Amount))]
        [MapProperty(nameof(CarDTO.IsAvailable), nameof(Car.IsAvailable))]
        public partial Car CarDtoToCar(CarDTO carDto);


        /// <summary>
        /// Маппинг с объекта автомобиля на DTO доступности
        /// </summary>
        /// <param name="car">Объект автомобиля</param>
        /// <returns>DTO доступности</returns>
        [MapProperty(nameof(Car.Id), nameof(CarAvailabilityDTO.Id))]
        [MapProperty(nameof(Car.IsAvailable), nameof(CarAvailabilityDTO.IsAvailable))]
        public partial CarAvailabilityDTO CarAvailabilityUpdateDTO(Car car);


        /// <summary>
        /// Маппинг с объекта автомобиля на DTO количества
        /// </summary>
        /// <param name="car">Объект автомобиля</param>
        /// <returns>DTO количества</returns>
        [MapProperty(nameof(Car.Id), nameof(CarAmountDTO.Id))]
        [MapProperty(nameof(Car.Amount), nameof(CarAmountDTO.Amount))]
        public partial CarAmountDTO CarAmountToDto(Car car);
    }
}
