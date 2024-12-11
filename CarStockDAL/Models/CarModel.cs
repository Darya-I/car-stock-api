﻿namespace CarStockDAL.Models
{
    public class CarModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public int CarId { get; set; } //required FK
        public Car Car { get; set; }
    }
}
