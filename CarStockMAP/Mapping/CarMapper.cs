using CarStockDAL.Models;
using CarStockMAP.DTO;
using Riok.Mapperly.Abstractions;


namespace CarStockMAP.Mapping
{
    [Mapper]
    public partial class CarMapper
    {      
        [MapProperty(nameof(Car.Brand.Name), nameof(CarDTO.BrandName))]
        [MapProperty(nameof(Car.CarModel.Name), nameof(CarDTO.CarModelName))]
        [MapProperty(nameof(Car.Color.Name), nameof(CarDTO.ColorName))]
        [MapProperty(nameof(Car.Id), nameof(CarDTO.Id))]
        public partial CarDTO MapToCarDto(Car car);

        //

        [MapProperty(nameof(CarDTO.BrandName), nameof(Car.Brand.Name))]
        [MapProperty(nameof(CarDTO.CarModelName), nameof(Car.CarModel.Name))]
        [MapProperty(nameof(CarDTO.ColorName), nameof(Car.Color.Name))]
        public partial Car MapToCar(CarDTO car);
    }
}
