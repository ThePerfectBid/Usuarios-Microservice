using MassTransit;
using MediatR;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Usuarios.Application.Commands;
using Usuarios.Application.DTOs;
using Usuarios.Application.Events;
using Usuarios.Domain.Events;
using Usuarios.Domain.Repositories;
using Usuarios.Domain.ValueObjects;

namespace Usuarios.Application.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public UpdateUserCommandHandler(IUserRepository userRepository, IPublishEndpoint publishEndpoint)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.id);
            if (user == null)
                throw new ArgumentException("El usuario no existe.");

            user.Update(
                new string(request.UserDto.Name),
                new string(request.UserDto.LastName),
                new string(request.UserDto.Address),
                new string(request.UserDto.Phone)
            );
            Console.WriteLine("Ejecutando actualización en MongoWriteUserRepository...");
            await _userRepository.UpdateAsync(user);

            var userUpdatedEvent = new UserUpdatedEvent(user.Id, user.Name, user.LastName, user.Address, user.Phone);
            await _publishEndpoint.Publish(userUpdatedEvent);
            await _publishEndpoint.Publish(new UserActivityMadeEvent(user.Id.Value, "USER_UPDATED", DateTime.UtcNow));

            return true;
        }
    }
}
