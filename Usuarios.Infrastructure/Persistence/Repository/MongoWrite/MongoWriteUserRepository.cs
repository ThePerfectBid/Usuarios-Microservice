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

namespace Usuarios.Infrastructure.Persistence.Repository.MongoWrite
{
    public class MongoWriteUserRepository : IUserRepository
    {
        private readonly IMongoCollection<BsonDocument> _usersCollection;
        private readonly IRoleRepository _roleRepository; // Agregado para obtener el objeto Role

        public MongoWriteUserRepository(MongoWriteDbConfig mongoConfig, IRoleRepository roleRepository)
        {
            _usersCollection = mongoConfig.db.GetCollection<BsonDocument>("usuarios_write");
            _roleRepository = roleRepository; // Inicialización del repositorio de roles
        }

        public async Task AddAsync(User user)
        {
            var bsonUser = new BsonDocument
            {
                { "_id", user.Id.Value },  // ID como String manejado por el sistema
                { "name", user.Name.Value },
                { "lastName", user.LastName.Value },
                { "email", user.Email.Value },
                { "address", user.Address?.Value ?? "" },
                { "phone", user.Phone?.Value ?? "" },
                { "roleId", user.RoleId.Value},  // Se almacena solo el ID del rol
                //{ "isActive", true },
                { "createdAt", DateTime.UtcNow },
                { "updatedAt", DateTime.UtcNow }
            };

            await _usersCollection.InsertOneAsync(bsonUser);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("email", email);
            var bsonUser = await _usersCollection.Find(filter).FirstOrDefaultAsync();

            if (bsonUser == null)
                return null;

            // 🔹 Obtener el objeto Role desde MongoDB usando el RoleRepository
            var roleId = new VORoleId(bsonUser["roleId"].AsString);

            return UserFactory.Create(
                new VOId(bsonUser["_id"].AsString),
                new VOName(bsonUser["name"].AsString),
                new VOLastName(bsonUser["lastName"].AsString),
                new VOEmail(bsonUser["email"].AsString),
                roleId, // Ahora se asigna el objeto RoleId correctamente
                new VOAddress(bsonUser["address"].AsString),
                new VOPhone(bsonUser["phone"].AsString)
            );
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
