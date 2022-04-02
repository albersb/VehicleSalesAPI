using VehicleSales.Api.Models;
using VehicleSales.Api.Repositories;
using VehicleSales.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace VehicleSales.Api.Controllers
{
    [ApiController]
    [Route("vehicles")]
    public class VehicleController : ControllerBase
    {
        private readonly VehicleRepo repo;
        private readonly ILogger<VehicleController> logger;

        public VehicleController(VehicleRepo repo, ILogger<VehicleController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<VehicleDto>> GetVehiclesAsync()
        {
            var vehicles = (await repo.GetVehiclesAsync()).Select( vehicle => vehicle.AsDto());
            logger.LogInformation(message: $"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {vehicles.Count()} vehicles");
            return vehicles;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> Id(Guid id)
        {
            var vehicle = await repo.Id(id);

            if(vehicle is null )
            {
                return NotFound();
            }

            return vehicle.AsDto();
        }

        [Route("[action]/{str}")]
        [HttpGet]
        public async Task<IEnumerable<Vehicle>> Make(string str)
        {
            var vehicle = await repo.Make(str);

            return vehicle;
        }

        [Route("[action]/{str}")]
        [HttpGet]
        public async Task<IEnumerable<Vehicle>> Model(string str)
        {
            var vehicle = await repo.Model(str);

            return vehicle;
        }

        [Route("[action]/{str}")]
        [HttpGet]
        public async Task<IEnumerable<Vehicle>> Year(string str)
        {
            var vehicle = await repo.Year(str);

            return vehicle;
        }

        [Route("[action]/{str}")]
        [HttpGet]
        public async Task<IEnumerable<Vehicle>> Color(string str)
        {
            var vehicle = await repo.Color(str);

            return vehicle;
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> CreateVehicleAsync(CreateVehicleDto vehicleDto)
        {
            Vehicle vehicle = new()
            {
                Id = Guid.NewGuid(),
                Make = vehicleDto.Make,
                Model = vehicleDto.Model,
                Year = vehicleDto.Year,
                Color = vehicleDto.Color,
                Price = vehicleDto.Price,
                Mileage = vehicleDto.Mileage
            };

            await repo.CreateVehicleAsync(vehicle);

            return CreatedAtAction(nameof(Id), new { id = vehicle.Id}, vehicle.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVehicleAsync(Guid id, UpdateVehicleDto vehicleDto)
        {
            var existingVehicle = await repo.Id(id);

            if(existingVehicle is null)
            {
                return NotFound();
            }
            existingVehicle.Make = vehicleDto.Make;
            existingVehicle.Model = vehicleDto.Model;
            existingVehicle.Year = vehicleDto.Year;
            existingVehicle.Color = vehicleDto.Color;
            existingVehicle.Price = vehicleDto.Price;
            existingVehicle.Mileage = vehicleDto.Mileage;

            await repo.UpdateVehicleAsync(existingVehicle);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingVehicle = await repo.Id(id);

            if(existingVehicle is null)
            {
                return NotFound();
            }

            await repo.DeleteVehicleAsync(id);

            return NoContent();
        }
    }
}