using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Transports;
using MediatR;
using Usuarios.Domain.Events;

namespace Usuarios.Application.EventHandlers
{
    public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
    {
        private readonly ISendEndpointProvider _publishEndpoint;

        public UserCreatedEventHandler(ISendEndpointProvider publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task Handle(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Usuario creado: {userCreatedEvent} con ID {userCreatedEvent}");

            await _publishEndpoint.Send(userCreatedEvent); //Publica el evento en RabbitMQ con MassTransit
        }


    }
}