using MassTransit;
using MongoDB.Bson;
using Usuarios.Application.EventHandlers;
using Usuarios.Domain.Aggregates;
using Usuarios.Domain.Events;
using Usuarios.Infrastructure.Interfaces;


//using RabbitMQ.Contracts;

namespace Application.Core
{
    public class CreateUserConsumer(IServiceProvider serviceProvider, IUserReadRepository userReadRepository) : IConsumer<UserCreatedEvent>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        private readonly IUserReadRepository _userReadRepository = userReadRepository;
        public async Task Consume(ConsumeContext<UserCreatedEvent> @event)
        {
            var message = @event.Message;
            Console.WriteLine($"Usuario creado: {message}");

            var bsonUser = new BsonDocument
            {
                { "_id", message.Id },
                { "name", message.Name},
                { "lastName", message.LastName},
                { "email", message.Email},
                { "address", message.Address},
                { "phone", message.Phone},
                { "roleId", message.RoleId}, 
            };

            await userReadRepository.AddAsync(bsonUser);

            return; //Task.CompletedTask;
        }
    }
}