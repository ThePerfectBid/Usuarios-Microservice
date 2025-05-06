using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Usuarios.Infrastructure.Persistence.Repository.MongoWrite.Documents
{
    public class UserMongoWrite
    {
        [BsonId]
        [BsonElement("id")]
        public required string Id { get; set; } // El ID es un string manejado por el sistema

        [BsonElement("name")]
        public required string Name { get; set; }

        [BsonElement("lastName")]
        public required string LastName { get; set; }

        [BsonElement("email")]
        public required string Email { get; set; }

        [BsonElement("address")]
        public string? Address { get; set; }

        [BsonElement("phone")]
        public string? Phone { get; set; }

        //[BsonElement("role")]
        //public RoleMongoWrite Role { get; set; } // Relación con el rol del usuario
    }

    //public class RoleMongoWrite
    //{
    //    [BsonElement("name")]
    //    public string Name { get; set; }

    //    [BsonElement("permissions")]
    //    public List<string> Permissions { get; set; }
    //}
}
