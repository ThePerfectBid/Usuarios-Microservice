using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Usuarios.Domain.Entities;
using Usuarios.Domain.Repositories;
using Usuarios.Domain.ValueObjects;
using Usuarios.Infrastructure.Configurations;

namespace Usuarios.Infrastructure.Persistence.Repository.MongoWrite
{
    public class RoleWriteRepository : IRoleRepository
    {
        private readonly IMongoCollection<BsonDocument> _rolesCollection;

        public RoleWriteRepository(MongoWriteDbConfig mongoConfig)
        {
            _rolesCollection = mongoConfig.db.GetCollection<BsonDocument>("Roles");
        }

        public async Task<Role?> GetByIdAsync(string roleId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", roleId);
            var bsonRole = await _rolesCollection.Find(filter).FirstOrDefaultAsync();

            if (bsonRole == null)
                return null;

            return new Role(
                new VORoleId(bsonRole["_id"].AsString),
                new VORoleName(bsonRole["RoleName"].AsString),
                new VORolePermissions(bsonRole["PermissionIds"].AsBsonArray.Select(p => p.AsString).ToList())
            );
        }

        public async Task<List<Role>> GetAllAsync()
        {
            var roles = await _rolesCollection.Find(new BsonDocument()).ToListAsync();
            return roles.Select(bsonRole => new Role(
                new VORoleId(bsonRole["_id"].AsString),
                new VORoleName(bsonRole["RoleName"].AsString),
                new VORolePermissions(bsonRole["PermissionIds"].AsBsonArray.Select(p => p.AsString).ToList())
            )).ToList();
        }

        //public async Task AddAsync(Role role)
        //{
        //    var bsonRole = new BsonDocument
        //    {
        //        { "_id", role.Id.Value },
        //        { "name", role.Name.Value },
        //        { "permissions", new BsonArray(role.Permissions.Permissions) }
        //    };

        //    await _rolesCollection.InsertOneAsync(bsonRole);
        //}

        public async Task<bool> UpdatePermissionsAsync(VORoleId roleId, VORolePermissions permissions)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", roleId.Value);
            var update = Builders<BsonDocument>.Update.Set("PermissionIds", new BsonArray(permissions.PermissionIds));

            var result = await _rolesCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        //public async Task<bool> DeleteAsync(VORoleId roleId)
        //{
        //    var filter = Builders<BsonDocument>.Filter.Eq("_id", roleId.Value);
        //    var result = await _rolesCollection.DeleteOneAsync(filter);

        //    return result.DeletedCount > 0;
        //}
    }
}
