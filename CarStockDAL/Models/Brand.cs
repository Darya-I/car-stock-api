namespace CarStockDAL.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Car> Cars { get; set; } = new List<Car>();
        public ICollection<CarModel> CarsModel { get; set; } = new List<CarModel>();

    }
}

// все коллекции инициализируются чтоб не было NullReferenceException