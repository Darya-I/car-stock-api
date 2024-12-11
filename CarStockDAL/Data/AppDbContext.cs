using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarStockDAL.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext() 
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CarModel> Models { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Car>()
                .HasOne(m => m.Model)
                .WithOne(m => m.Car)
                .HasForeignKey<Car>(m => m.ModelId);

            modelBuilder.Entity<Car>()
                .HasOne(b => b.Brand)
                .WithOne(b => b.Car)
                .HasForeignKey<Car>(b => b.BrandId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Color)
                .WithOne(c => c.Car)
                .HasForeignKey<Car>(m => m.ColorId);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Brand>().HasData(
                new Brand { Id = 1, Name = "Toyota" },
                new Brand { Id = 2, Name = "BMW" },
                new Brand { Id = 3, Name = "Porsche" },
                new Brand { Id = 4, Name = "Mercedes" },
                new Brand { Id = 5, Name = "Audi" }
            );

            modelBuilder.Entity<CarModel>().HasData(
                // Toyota Models
                new CarModel { Id = 1, Name = "Corolla"},
                new CarModel { Id = 2, Name = "Camry"},
                new CarModel { Id = 3, Name = "RAV4" },
                // BMW Models
                new CarModel { Id = 4, Name = "X5" },
                new CarModel { Id = 5, Name = "3 Series"}, 
                new CarModel { Id = 6, Name = "5 Series"},
                // Porsche Models
                new CarModel { Id = 7, Name = "911 Carrera"},
                new CarModel { Id = 8, Name = "Cayenne"},
                new CarModel { Id = 9, Name = "Macan"},
                // Mercedes Models
                new CarModel { Id = 10, Name = "C-Class"},
                new CarModel { Id = 11, Name = "E-Class"},
                new CarModel { Id = 12, Name = "GLE"},
                // Audi Models
                new CarModel { Id = 13, Name = "A4"},
                new CarModel { Id = 14, Name = "Q5"},
                new CarModel { Id = 15, Name = "A6"}
            );

            modelBuilder.Entity<Color>().HasData(
                new Color { Id = 1, Name = "Red" },
                new Color { Id = 2, Name = "Blue" },
                new Color { Id = 3, Name = "Black" },
                new Color { Id = 4, Name = "White" },
                new Color { Id = 5, Name = "Silver" },
                new Color { Id = 6, Name = "Green" },
                new Color { Id = 7, Name = "Yellow" },
                new Color { Id = 8, Name = "Gray" }
            );


        }
    }
}


//dotnet ef migrations add SeedData
//dotnet ef database update
