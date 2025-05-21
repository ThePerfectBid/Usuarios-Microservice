using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Infrastructure.Persistence.Repository.MongoRead.Documents;

namespace Usuarios.Infrastructure.Interfaces
{
    public interface IUserActivityReadRepository
    {
        Task AddAsync(BsonDocument userActivity);
        //Task<BsonDocument> GetByIdAsync(string id);
        Task<List<BsonDocument>> GetAllAsync();
        Task<List<BsonDocument>> GetByUserIdAsync(string userId, DateTime timestamp); //, DateTime start, DateTime end



    }
}