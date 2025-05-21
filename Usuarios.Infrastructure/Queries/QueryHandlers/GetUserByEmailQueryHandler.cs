using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios.Infrastructure.Queries.QueryHandlers
{
    using MediatR;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Usuarios.Application.DTOs;
    using Usuarios.Infrastructure.Interfaces;

    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserByEmailDto>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetUserByEmailQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<UserByEmailDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado.");
            }

            var userDto = new UserByEmailDto
            {
                userId = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                RoleId = user.roleId,
                Address = user.Address,
                Phone = user.Phone
            };

            return userDto;
        }
    }
}
