using MassTransit;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Application.Events;
using Usuarios.Domain.Events;
using Usuarios.Infrastructure.Interfaces;

namespace Usuarios.Infrastructure.Consumer
{
    public class UserActivityConsumer(IServiceProvider serviceProvider, IUserActivityReadRepository userActivityReadRepository) : IConsumer<UserActivityMadeEvent>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        private readonly IUserActivityReadRepository _userReadRepository = userActivityReadRepository;

        public async Task Consume(ConsumeContext<UserActivityMadeEvent> @event)
        {
            var message = @event.Message;
            Console.WriteLine($"Historial actualizado: {message}");

            var bsonUser = new BsonDocument
            {
                { "_id", (Guid.NewGuid().ToString()) },
                { "_userid", message.UserId },
                { "message", message.Action},
                { "timestamp", message.Timestamp}
            };

            await userActivityReadRepository.AddAsync(bsonUser);

            return; //Task.CompletedTask;
        }
    }
}
