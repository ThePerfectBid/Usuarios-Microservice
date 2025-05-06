using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Usuarios.Infrastructure.Configurations
{
    public class MongoReadDbConfig
    {
        public MongoClient client;
        public IMongoDatabase db;

        public MongoReadDbConfig()
        {
            // Obtener la cadena de conexión de la variable de entorno o usar un valor por defecto
            string connectionUri = Environment.GetEnvironmentVariable("MONGODB_CNN_READ");

            if (string.IsNullOrWhiteSpace(connectionUri))
                throw new ArgumentException("La cadena de conexión de MongoDB no está definida.");

            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            client = new MongoClient(settings);
            db = client.GetDatabase(Environment.GetEnvironmentVariable("MONGODB_NAME_READ"));
        }
    }
}