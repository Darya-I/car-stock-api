using CarStockDAL.Models;
using CarStockMAP.DTO;
using CarStockMAP.ViewModels;
using Riok.Mapperly.Abstractions;


namespace CarStockMAP.Mapping
{
    [Mapper]
    public partial class CarMapper
    {
        //public partial CarDTO MapToCar(IEnumerable<Car> cars);
        
        [MapProperty(nameof(Car.Brand.Name), nameof(CarDTO.BrandName))]
        [MapProperty(nameof(Car.CarModel.Name), nameof(CarDTO.CarModelName))]
        [MapProperty(nameof(Car.Color.Name), nameof(CarDTO.ColorName))]
        [MapProperty(nameof(Car.Id), nameof(CarDTO.Id))]
        public partial CarDTO MapToCarDto(Car car);


        //[MapProperty(nameof(CarViewModel.BrandName), (nameof(Car.Brand.Name), nameof(Car.Brand.Id))]
        //public partial Car MapToCar(CarViewModel car);
    }
}
