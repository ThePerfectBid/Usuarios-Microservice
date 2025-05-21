using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Application.DTOs;

namespace Usuarios.Infrastructure.Queries
{
    public class GetUserByEmailQuery : IRequest<UserByEmailDto>
    {
        public string Email { get; set; }

        public GetUserByEmailQuery(string email)
        {
            Email = email;
        }
    }
}
