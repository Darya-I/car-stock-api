using CarStockDAL.Models;
using CarStockMAP.DTO.Car;
using Riok.Mapperly.Abstractions;


namespace CarStockMAP.Mapping
{
    [Mapper]
    public partial class CarMapper
    {      
        /// <summary>
        /// Маппинг из Car в CarDTO
        /// </summary>
        [MapProperty(nameof(Car.Brand.Name), nameof(CarDTO.BrandName))]
        [MapProperty(nameof(Car.CarModel.Name), nameof(CarDTO.CarModelName))]
        [MapProperty(nameof(Car.Color.Name), nameof(CarDTO.ColorName))]
        [MapProperty(nameof(Car.Id), nameof(CarDTO.Id))]
        public partial CarDTO MapCarToCarDto(Car car);

        /// <summary>
        /// Маппинг из CarDTO в Car
        /// </summary>
        [MapProperty(nameof(CarDTO.BrandName), nameof(Car.Brand.Name))]
        [MapProperty(nameof(CarDTO.CarModelName), nameof(Car.CarModel.Name))]
        [MapProperty(nameof(CarDTO.ColorName), nameof(Car.Color.Name))]
        public partial Car MapCarDTOToCar(CarDTO car);

        /// <summary>
        /// Маппинг из UpdateCarDTO в Car
        /// </summary>
        [MapProperty(nameof(CarUpdateDTO.Id), nameof(Car.Id))]
        [MapProperty(nameof(CarUpdateDTO.BrandId), nameof(Car.BrandId))]
        [MapProperty(nameof(CarUpdateDTO.CarModelId), nameof(Car.CarModelId))]
        [MapProperty(nameof(CarUpdateDTO.ColorId), nameof(Car.ColorId))]
        [MapProperty(nameof(CarUpdateDTO.Amount), nameof(Car.Amount))]
        [MapProperty(nameof(CarUpdateDTO.IsAvaible), nameof(Car.IsAvaible))]
        public partial Car MapUpdateCarDtoToCar(CarUpdateDTO carUpdateDTO);
    }
}
