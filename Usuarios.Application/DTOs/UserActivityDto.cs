using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios.Application.DTOs
{
    public class UserActivityDto
    {
        public string Id { get; set; } = default!;
        public required string UserId { get; set; } = default!;
        public required string Action { get; set; } = default!;
        public DateTime Timestamp { get; set; }
    }
}
