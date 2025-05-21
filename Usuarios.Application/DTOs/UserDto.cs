using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios.Application.DTOs
{
    public class UserDto
    {
        public string Name { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string RoleId { get; set; }
        public string Address { get; init; }
        public string Phone { get; init; }
    }
}
