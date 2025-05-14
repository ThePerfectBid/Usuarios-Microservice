using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios.Application.DTOs
{
    public class UpdateUserDto
    {
        public string? Name { get; init; }
        public string? LastName { get; init; }
        public string? Address { get; init; }
        public string? Phone { get; init; }
    }
}
