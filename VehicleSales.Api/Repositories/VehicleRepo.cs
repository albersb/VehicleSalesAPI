using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleSales.Api.Models;

namespace VehicleSales.Api.Repositories
{
    public interface VehicleRepo
    {
        Task<Vehicle> Id(Guid id);
        Task<IEnumerable<Vehicle>> GetVehiclesAsync();
        Task<IEnumerable<Vehicle>> Make(string str);
        Task<IEnumerable<Vehicle>> Model(string str);
        Task<IEnumerable<Vehicle>> Year(string str);
        Task<IEnumerable<Vehicle>> Color(string str);
        Task CreateVehicleAsync(Vehicle vehicle);

        Task UpdateVehicleAsync(Vehicle vehicle);

        Task DeleteVehicleAsync(Guid id);
    }
}