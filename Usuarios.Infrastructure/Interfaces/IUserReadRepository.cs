using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Domain.Aggregates;
using MongoDB.Bson;
using Usuarios.Infrastructure.Persistence.Repository.MongoRead.Documents;

namespace Usuarios.Infrastructure.Interfaces
{
    public interface IUserReadRepository
    {
        Task AddAsync(BsonDocument User);
        Task<UserMongoRead> GetByEmailAsync(string email);
    }
}

