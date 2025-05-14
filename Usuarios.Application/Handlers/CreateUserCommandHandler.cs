using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Usuarios.Application.Commands;
using Usuarios.Domain.Factories;
using Usuarios.Domain.Repositories;
using Usuarios.Application.DTOs;
using Usuarios.Domain.ValueObjects;
using Usuarios.Domain.Aggregates;
using Usuarios.Domain.Entities;
using Usuarios.Domain.Events;

namespace Usuarios.Application.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, VOId>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMediator _mediator;

        public CreateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IMediator mediator)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<VOId> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Verificar si el ID del rol existe en la base de datos
            var roleExists = await _roleRepository.GetByIdAsync(request.UserDto.RoleId);
            if (roleExists == null)
                throw new ArgumentException("El rol especificado no existe.");

            // Crear el usuario solo con el ID del rol
            var user = new User(
                new VOId(Guid.NewGuid().ToString()),
                new VOName(request.UserDto.Name),
                new VOLastName(request.UserDto.LastName),
                new VOEmail(request.UserDto.Email),
                new VORoleId(request.UserDto.RoleId), //Solo se almacena el ID del rol existente
                new VOAddress(request.UserDto.Address),
                new VOPhone(request.UserDto.Phone)
            );

            await _userRepository.AddAsync(user);
            var userCreatedEvent = new UserCreatedEvent(user.Id.Value, user.Name.Value, user.LastName.Value, user.Email.Value, user.RoleId.Value, user.Address?.Value, user.Phone?.Value);
            await _mediator.Publish(userCreatedEvent); // Publicar el evento de usuario creado

            return user.Id; // Extrae el valor primitivo antes de retornarlo
        }
    }
}
