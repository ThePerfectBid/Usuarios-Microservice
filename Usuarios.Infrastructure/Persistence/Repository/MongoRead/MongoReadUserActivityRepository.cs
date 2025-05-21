using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Domain.Repositories;
using Usuarios.Infrastructure.Configurations;
using Usuarios.Infrastructure.Interfaces;

namespace Usuarios.Infrastructure.Persistence.Repository.MongoRead
{
    public class MongoReadUserActivityRepository : IUserActivityReadRepository
    {
        private readonly IMongoCollection<BsonDocument> _usersActivityCollection;
        //private readonly IRoleRepository _roleRepository; // Agregado para obtener el objeto Role

        public MongoReadUserActivityRepository(MongoReadUserActivityDbConfig mongoConfig, IRoleRepository roleRepository)
        {
            _usersActivityCollection = mongoConfig.db.GetCollection<BsonDocument>("Historico");
            //_roleRepository = roleRepository; // Inicialización del repositorio de roles
        }

        public async Task AddAsync(BsonDocument userActivity)
        {


            await _usersActivityCollection.InsertOneAsync(userActivity);
        }

        public async Task<List<BsonDocument>> GetAllAsync()
        {
            var bsonUserActivity = await _usersActivityCollection.Find(new BsonDocument()).ToListAsync();
            if (bsonUserActivity == null)
                return null;
            return bsonUserActivity;
        }

        public async Task<List<BsonDocument>> GetByUserIdAsync(string userId, DateTime timestamp) //, DateTime start, DateTime end
        {
            var filter = Builders<BsonDocument>.Filter.And(
                //Builders<BsonDocument>.Filter.Eq("_id", id),
                Builders<BsonDocument>.Filter.Eq("_userid", userId),
                Builders<BsonDocument>.Filter.Gte("timestamp", timestamp)
            //Builders<BsonDocument>.Filter.Lte("timestamp", end )
            );
            var bsonUserActivity = await _usersActivityCollection.Find(filter).ToListAsync();
            if (bsonUserActivity == null)
                return null;
            return bsonUserActivity;
        }
    }
}
