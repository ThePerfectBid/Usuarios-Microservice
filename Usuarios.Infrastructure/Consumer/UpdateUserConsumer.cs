using MassTransit;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Domain.Events;
using Usuarios.Infrastructure.Interfaces;

namespace Usuarios.Infrastructure.Consumer
{
    public class UpdateUserConsumer(IServiceProvider serviceProvider, IUserReadRepository userReadRepository) : IConsumer<UserUpdatedEvent>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        private readonly IUserReadRepository _userReadRepository = userReadRepository;
        public async Task Consume(ConsumeContext<UserUpdatedEvent> @event)
        {
            var message = @event.Message;
            Console.WriteLine($"Usuario creado: {message}");

            var bsonUser = new BsonDocument
            {
                { "_id", message.UserId.Value},
                { "name", message.Name.Value},
                { "lastName", message.LastName.Value},
                { "address", message.Address.Value},
                { "phone", message.Phone.Value}
            };

            await userReadRepository.UpdateAsync(bsonUser);

            return; //Task.CompletedTask;
        }
    }
}