using VehicleSales.Api.Dtos;
using VehicleSales.Api.Models;

namespace VehicleSales.Api
{
    public static class Extensions
    {
        public static VehicleDto AsDto(this Vehicle vehicle)
        {
            return new VehicleDto(vehicle.Id, vehicle.Make, vehicle.Model, vehicle.Year, vehicle.Color, vehicle.Price, vehicle.Mileage);
        }
    }
}