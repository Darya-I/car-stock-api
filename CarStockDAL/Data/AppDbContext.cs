using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CarStockDAL.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {

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
 
        }
    }
}


//dotnet ef migrations add SeedData
//dotnet ef database update
// https://learn.microsoft.com/ru-ru/ef/core/modeling/relationships/one-to-many