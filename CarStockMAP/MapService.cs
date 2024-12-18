using CarStockBLL.Interfaces;
using CarStockMAP.DTO;
using CarStockMAP.Mapping;
using CarStockMAP.ViewModels;


namespace CarStockMAP
{
    public class MapService
    {
        public readonly ICarService _carService;
        //public readonly IBrandService _brandService;

        public MapService(ICarService carService)
        {
            _carService = carService;
           // _brandService = brandService;
        }

        public async Task<IEnumerable<CarDTO>> GetMappedCarsAsync()
        {
            var cars = await _carService.GetAllCarsAsync();

            var mapper = new CarMapper();
            //преобразуем каждый объект Car из коллекции cars в объекты CarDto и создаем список
            var carDtos = cars.Select(car => mapper.MapToCarDto(car)).ToList();  

            return carDtos;

        }

        //принимаем CarDto от клиента
        public async Task CreateCarAsync(CarViewModel car)
        {
            try
            {
                return;
                var mapper = new CarMapper();
                //var car = mapper.MapToCar(carDTO);
                //await _carService.CreateCarAsync(car);

            }
            catch (Exception ex) 
            {
                return;
            }

        }

    }
}
