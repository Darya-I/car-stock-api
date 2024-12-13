﻿using CarStockDAL.Models;
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
                .HasOne(c => c.Brand)
                .WithMany(c => c.Cars)
                .HasForeignKey(c => c.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CarModel>()
                .HasOne(m => m.Brand)
                .WithMany(b => b.CarsModel)
                .HasForeignKey(m => m.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Color>()
                .HasOne(m => m.CarModel)
                .WithMany(c => c.Colors)
                .HasForeignKey(m => m.CarModelId)
                .OnDelete(DeleteBehavior.Cascade);

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
                new CarModel { Id = 1, Name = "Corolla", BrandId = 1},
                new CarModel { Id = 2, Name = "Camry", BrandId = 1 },
                new CarModel { Id = 3, Name = "RAV4", BrandId = 1 },
                // BMW Models
                new CarModel { Id = 4, Name = "X5" , BrandId = 2},
                new CarModel { Id = 5, Name = "3 Series", BrandId = 2 }, 
                new CarModel { Id = 6, Name = "5 Series", BrandId = 2 },
                // Porsche Models
                new CarModel { Id = 7, Name = "911 Carrera", BrandId = 3 },
                new CarModel { Id = 8, Name = "Cayenne", BrandId = 3 },
                new CarModel { Id = 9, Name = "Macan", BrandId = 3 },
                // Mercedes Models
                new CarModel { Id = 10, Name = "C-Class", BrandId = 4 },
                new CarModel { Id = 11, Name = "E-Class", BrandId = 4 },
                new CarModel { Id = 12, Name = "GLE", BrandId = 4 },
                // Audi Models
                new CarModel { Id = 13, Name = "A4", BrandId = 4 },
                new CarModel { Id = 14, Name = "Q5", BrandId = 4 },
                new CarModel { Id = 15, Name = "A6", BrandId = 4 }
            );

            modelBuilder.Entity<Color>().HasData(
                new Color { Id = 1, Name = "Black", CarModelId = 4 },
                new Color { Id = 2, Name = "White", CarModelId = 4 },

                new Color { Id = 3, Name = "Grey", CarModelId = 5 },
                new Color { Id = 4, Name = "Blue", CarModelId = 5 },

                new Color { Id = 5, Name = "Silver", CarModelId = 6 },
                new Color { Id = 6, Name = "Black", CarModelId = 6 },

                new Color { Id = 7, Name = "Red", CarModelId = 7 },
                new Color { Id = 8, Name = "Yellow", CarModelId = 7 },

                new Color { Id = 9, Name = "White", CarModelId = 8 },
                new Color { Id = 10, Name = "Red", CarModelId = 8 },

                new Color { Id = 11, Name = "Green", CarModelId = 9 },
                new Color { Id = 12, Name = "Blue", CarModelId = 9 }
            );

        }
    }
}


//dotnet ef migrations add SeedData
//dotnet ef database update
// https://learn.microsoft.com/ru-ru/ef/core/modeling/relationships/one-to-many