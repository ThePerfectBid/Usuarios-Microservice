using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Usuarios.Domain.Aggregates;
using Usuarios.Domain.Entities;
using Usuarios.Domain.Repositories;
using MongoDB.Bson;
using Usuarios.Infrastructure.Configurations;
using System.Data;
using Usuarios.Domain.Factories;
using Usuarios.Domain.ValueObjects;
using Usuarios.Infrastructure.Interfaces;
using Usuarios.Infrastructure.Persistence.Repository.MongoRead.Documents;

namespace Usuarios.Infrastructure.Persistence.Repository.MongoWrite
{
    public class MongoReadUserRepository : IUserReadRepository
    {
        private readonly IMongoCollection<BsonDocument> _usersCollection;
        private readonly IRoleRepository _roleRepository; // Agregado para obtener el objeto Role

        public MongoReadUserRepository(MongoReadDbConfig mongoConfig, IRoleRepository roleRepository)
        {
            _usersCollection = mongoConfig.db.GetCollection<BsonDocument>("usuarios");
            _roleRepository = roleRepository; // Inicialización del repositorio de roles
        }

        public async Task AddAsync(BsonDocument user)
        {


            await _usersCollection.InsertOneAsync(user);
        }

        public async Task<UserMongoRead?> GetByEmailAsync(string email)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("email", email);
            var bsonUser = await _usersCollection.Find(filter).FirstOrDefaultAsync();

            if (bsonUser == null)
                return null;

            // 🔹 Obtener el objeto Role desde MongoDB usando el RoleRepository


            return new UserMongoRead
            {
                Id = bsonUser["_id"].AsString,
                Name = bsonUser["name"].AsString,
                LastName = bsonUser["lastName"].AsString,
                Email = bsonUser["email"].AsString,
                roleId = bsonUser["roleId"].AsString,
                Address = bsonUser["address"].AsString,
                Phone = bsonUser["phone"].AsString
            };
        }
    }

        //public async Task<bool> UpdateUserById(string userId, string? name, string? lastName, string? phone)
        //{
        //    var filter = Builders<BsonDocument>.Filter.Eq("_id", userId);
        //    var update = Builders<BsonDocument>.Update.Set("updatedAt", DateTime.UtcNow);

        //    if (!string.IsNullOrWhiteSpace(name))
        //        update = update.Set("name", name);

        //    if (!string.IsNullOrWhiteSpace(lastName))
        //        update = update.Set("lastName", lastName);

        //    if (!string.IsNullOrWhiteSpace(phone))
        //        update = update.Set("phone", phone);

        //    var result = await _usersCollection.UpdateOneAsync(filter, update);
        //    return result.ModifiedCount > 0;
        //}

        //public async Task<bool> ToggleActivityUserById(string userId)
        //{
        //    var filter = Builders<BsonDocument>.Filter.Eq("_id", userId);
        //    var user = await _usersCollection.Find(filter).FirstOrDefaultAsync();

        //    if (user == null)
        //        return false;

        //    bool isActive = !user["isActive"].AsBoolean;
        //    var update = Builders<BsonDocument>.Update.Set("isActive", isActive).Set("updatedAt", DateTime.UtcNow);

        //    var result = await _usersCollection.UpdateOneAsync(filter, update);
        //    return result.ModifiedCount > 0;
        //}
}
