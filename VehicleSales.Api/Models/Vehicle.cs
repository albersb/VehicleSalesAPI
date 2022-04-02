using System;

namespace VehicleSales.Api.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }

        public string Make { get; set;}

        public string Model { get; set;}

        public string Year { get; set;}

        public string Color { get; set;}

        public decimal Price { get; set;}

        public string Mileage { get; set;}
    }
}