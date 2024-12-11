using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;
namespace CarStockDAL.Data
{
    public class PostgreCarRepository : ICarRepository<Car>
    {
        private AppDbContext db;

        public PostgreCarRepository(AppDbContext db) 
        {
            this.db = db;
        }

        public void Create(Car car)
        {
            db.Cars.Add(car);
        }

        public void Delete(int id)
        {
            Car car = db.Cars.Find(id);
            if (car != null) 
            {
                db.Cars.Remove(car);
            }
        }

        public IEnumerable<Car> GetAllCars()
        {
            return db.Cars;
        }

        public Car GetCar(int id)
        {
            return db.Cars.Find(id);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Car car)
        {
            db.Entry(car).State = EntityState.Modified;
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing) 
        {
            if (!this.disposed)
            {
                if (disposing) 
                {
                    db.Dispose();
                }
            }
            this.disposed = true;                           //Флаг, предотвращающий повторное освобождение ресурсов.
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);                      //Указывает сборщику мусора, что финализатор для данного объекта можно пропустить,
                                                            //так как ресурсы уже освобождены вручную.
        }
    }
}
