using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Transports;
using MediatR;
using Usuarios.Domain.Events;

namespace Usuarios.Domain.Events.EventHandler
{
    public class UserUpdatedEventHandler : INotificationHandler<UserUpdatedEvent>
    {
        private readonly ISendEndpointProvider _publishEndpoint;

        public UserUpdatedEventHandler(ISendEndpointProvider publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task Handle(UserUpdatedEvent userUpdatedEvent, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Usuario creado: {userUpdatedEvent} con ID {userUpdatedEvent}");

            await _publishEndpoint.Send(userUpdatedEvent); // 🔹 Publica el evento en RabbitMQ con MassTransit
        }


    }
}
