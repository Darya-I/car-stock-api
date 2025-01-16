﻿using System.Text.Json.Serialization;

namespace CarStockMAP.DTO.Car
{
    /// <summary>
    /// DTO автомобиля
    /// </summary>
    public class CarDTO
    {
        //public int Id { get; set; }
        public string BrandName { get; set; }
        public string CarModelName { get; set; }
        public string ColorName { get; set; }
        public int Amount { get; set; }
        public bool IsAvailable { get; set; }
    }
}