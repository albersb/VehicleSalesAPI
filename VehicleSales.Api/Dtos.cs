using System;
using System.ComponentModel.DataAnnotations;

namespace VehicleSales.Api.Dtos
{
    public record VehicleDto(Guid Id, string Make, string Model, string Year, string Color, decimal Price, string Mileage);

    public record CreateVehicleDto([Required] string Make, [Required] string Model, [Required] string Year, [Required] string Color, [Required][Range(1, Double.MaxValue)] decimal Price, [Required] string Mileage);

    public record UpdateVehicleDto([Required] string Make, [Required] string Model, [Required] string Year, [Required] string Color, [Required][Range(1, Double.MaxValue)] decimal Price, [Required] string Mileage);
}