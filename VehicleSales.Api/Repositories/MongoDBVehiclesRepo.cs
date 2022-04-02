using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using VehicleSales.Api.Models;

namespace VehicleSales.Api.Repositories
{
    public class MongoDBVehiclesRepo : VehicleRepo
    {
        private const string databaseName = "vehicle";
        private const string collectionName = "vehicles";
        private readonly IMongoCollection<Vehicle> vehicleCollection;
        private readonly FilterDefinitionBuilder<Vehicle> filterBuilder = Builders<Vehicle>.Filter;

        public MongoDBVehiclesRepo(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            vehicleCollection = database.GetCollection<Vehicle>(collectionName);
        }

        public async Task CreateVehicleAsync(Vehicle vehicle)
        {
            await vehicleCollection.InsertOneAsync(vehicle);
        }

        public async Task DeleteVehicleAsync(Guid id)
        {
            var filter = filterBuilder.Eq(vehicle => vehicle.Id, id);
            await vehicleCollection.DeleteOneAsync(filter);
        }

        public async Task<Vehicle> Id(Guid id)
        {
            var filter = filterBuilder.Eq(vehicle => vehicle.Id, id);
            return await vehicleCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Vehicle>> Make(string str)
        {
            return await vehicleCollection.Find(vehicle => vehicle.Make.ToLower() == str.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> Model(string str)
        {
            return await vehicleCollection.Find(vehicle => vehicle.Model.ToLower() == str.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> Year(string str)
        {
            return await vehicleCollection.Find(vehicle => vehicle.Year.ToLower() == str.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> Color(string str)
        {
            return await vehicleCollection.Find(vehicle => vehicle.Color.ToLower() == str.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync()
        {
            return await vehicleCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            var filter = filterBuilder.Eq(existingVehicle => existingVehicle.Id, vehicle.Id);
            await vehicleCollection.ReplaceOneAsync(filter,vehicle);
        }
    }
}