using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VehicleSales.Api.Controllers;
using VehicleSales.Api.Dtos;
using VehicleSales.Api.Models;
using VehicleSales.Api.Repositories;
using Xunit;

namespace VehicleSales.UnitTests;

public class VehicleControllerTests
{
    private readonly Mock<VehicleRepo> repositoryStub = new();
    private readonly Mock<ILogger<VehicleController>> loggerStub = new();
    private readonly Random rand = new();

    [Fact]
    public async Task Id_WithNonexistingVehicle_ReturnsNotFound()
    {
        // Arrange
        repositoryStub.Setup(repo => repo.Id(It.IsAny<Guid>())).ReturnsAsync((Vehicle)null);

        var controller = new VehicleController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.Id(Guid.NewGuid());

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Id_WithExistingVehicle_ReturnsExpectedVehicle()
    {
        // Arrange
        Vehicle expectedVehicle = CreateRandomVehicle();

        repositoryStub.Setup(repo => repo.Id(It.IsAny<Guid>())).ReturnsAsync(expectedVehicle);

        var controller = new VehicleController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.Id(Guid.NewGuid());

        // Assert
        result.Value.Should().BeEquivalentTo(expectedVehicle);
    }

    [Fact]
    public async Task Make_WithExistingVehicle_ReturnsExpectedVehicle()
    {
        // Arrange
        var allVehicles = new[]
        {
        new Vehicle(){ Make = "Jeep"},
        new Vehicle(){ Make = "Dodge"},
        new Vehicle(){ Make = "Ford"}
        };

        var makeToMatch = "Ford";

        repositoryStub.Setup(repo => repo.Make(makeToMatch)).ReturnsAsync(allVehicles);

        var controller = new VehicleController(repositoryStub.Object, loggerStub.Object);

        // Act
        IEnumerable<Vehicle> foundVehicles = await controller.Make(makeToMatch);

        // Assert
        foundVehicles.Should().OnlyContain( vehicle => vehicle.Make == allVehicles[0].Make || vehicle.Make == allVehicles[1].Make || vehicle.Make == allVehicles[2].Make );
    }

    [Fact]
    public async Task Model_WithExistingVehicle_ReturnsExpectedVehicle()
    {
        // Arrange
        var allVehicles = new[]
        {
        new Vehicle(){ Model = "Wrangler"},
        new Vehicle(){ Model = "Ram"},
        new Vehicle(){ Model = "Focus"}
        };

        var modelToMatch = "Ram";

        repositoryStub.Setup(repo => repo.Model(modelToMatch)).ReturnsAsync(allVehicles);

        var controller = new VehicleController(repositoryStub.Object, loggerStub.Object);

        // Act
        IEnumerable<Vehicle> foundVehicles = await controller.Model(modelToMatch);

        // Assert
        foundVehicles.Should().OnlyContain( vehicle => vehicle.Model == allVehicles[0].Model || vehicle.Model == allVehicles[1].Model || vehicle.Model == allVehicles[2].Model);
    }

    [Fact]
    public async Task Year_WithExistingVehicle_ReturnsExpectedVehicle()
    {
        // Arrange
        var allVehicles = new[]
        {
        new Vehicle(){ Year = "2015"},
        new Vehicle(){ Year = "2016"},
        new Vehicle(){ Year = "2017"}
        };

        var yearToMatch = "2015";

        repositoryStub.Setup(repo => repo.Year(yearToMatch)).ReturnsAsync(allVehicles);

        var controller = new VehicleController(repositoryStub.Object, loggerStub.Object);

        // Act
        IEnumerable<Vehicle> foundVehicles = await controller.Year(yearToMatch);

        // Assert
        foundVehicles.Should().OnlyContain( vehicle => vehicle.Year == allVehicles[0].Year || vehicle.Year == allVehicles[1].Year || vehicle.Year == allVehicles[2].Year );
    }

    [Fact]
    public async Task Color_WithExistingVehicle_ReturnsExpectedVehicle()
    {
        // Arrange
        var allVehicles = new[]
        {
        new Vehicle(){ Color = "Red"},
        new Vehicle(){ Color = "White"},
        new Vehicle(){ Color = "Black"}
        };

        var colorToMatch = "Black";

        repositoryStub.Setup(repo => repo.Color(colorToMatch)).ReturnsAsync(allVehicles);

        var controller = new VehicleController(repositoryStub.Object, loggerStub.Object);

        // Act
        IEnumerable<Vehicle> foundVehicles = await controller.Color(colorToMatch);

        // Assert
        foundVehicles.Should().OnlyContain( vehicle => vehicle.Color == allVehicles[0].Color || vehicle.Color == allVehicles[1].Color || vehicle.Color == allVehicles[2].Color );
    }

    [Fact]
    public async Task GetVehicleAsync_WithExistingVehicles_ReturnsAllVehicles()
    {
        // Arrange
        var expectedItems = new[] { CreateRandomVehicle(), CreateRandomVehicle(), CreateRandomVehicle() };

        repositoryStub.Setup(repo => repo.GetVehiclesAsync()).ReturnsAsync(expectedItems);

        var controller = new VehicleController(repositoryStub.Object, loggerStub.Object);

        // Act
        var actualItems = await controller.GetVehiclesAsync();

        // Assert
        actualItems.Should().BeEquivalentTo(expectedItems);
    }

    [Fact]
    public async Task CreateVehicleAsync_WithVehicleToCreate_ReturnsCreatedVehicle()
    {
        // Arrange
        var vehicleToCreate = new CreateVehicleDto(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            rand.Next(Int32.MaxValue),
            Guid.NewGuid().ToString());

        var controller = new VehicleController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.CreateVehicleAsync(vehicleToCreate);

        // Assert
        var createdItem = (result.Result as CreatedAtActionResult).Value as VehicleDto;
        vehicleToCreate.Should().BeEquivalentTo(
            createdItem,
            options => options.ComparingByMembers<VehicleDto>().ExcludingMissingMembers()
        );
        createdItem.Id.Should().NotBeEmpty();
    }

    [Fact]
        public async Task UpdateVehicleAsync_WithExistingVehicle_ReturnsNoContent()
        {
            // Arrange
            Vehicle existingVehicle= CreateRandomVehicle();
            repositoryStub.Setup(repo => repo.Id(It.IsAny<Guid>())).ReturnsAsync(existingVehicle);

            var itemId = existingVehicle.Id;
            var itemToUpdate = new UpdateVehicleDto(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                existingVehicle.Price + 3,
                Guid.NewGuid().ToString());

            var controller = new VehicleController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.UpdateVehicleAsync(itemId, itemToUpdate);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteVehicleAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            Vehicle existingVehicle = CreateRandomVehicle();
            repositoryStub.Setup(repo => repo.Id(It.IsAny<Guid>())).ReturnsAsync(existingVehicle);

            var controller = new VehicleController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.DeleteItemAsync(existingVehicle.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

    private Vehicle CreateRandomVehicle()
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Make = Guid.NewGuid().ToString(),
            Model = Guid.NewGuid().ToString(),
            Year = Guid.NewGuid().ToString(),
            Color = Guid.NewGuid().ToString(),
            Price = rand.Next(Int32.MaxValue),
            Mileage = Guid.NewGuid().ToString()
        };
    }
}