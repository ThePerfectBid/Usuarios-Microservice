using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Usuarios.Application.DTOs;
using Usuarios.Domain.ValueObjects;

namespace Usuarios.Application.Commands
{
    using MediatR;
    using Usuarios.Application.DTOs;

    public class CreateUserCommand : IRequest<VOId> //aqui estaria calidad que se devuelva algo (yo devolvia el ID jose)
    {
        public CreateUserDto UserDto { get; }

        public CreateUserCommand(CreateUserDto userDto)
        {
            UserDto = userDto ?? throw new ArgumentNullException(nameof(userDto));
        }
    }
}
