using MediatR;
using System;
using System.Collections.Generic;
using Usuarios.Application.DTOs;

namespace Usuarios.Infrastructure.Queries
{

    // Query definition
    public class UserActivityQuery : IRequest<IEnumerable<UserActivityDto>>
    {
        public string? UserId { get; set; }
        public DateTime Timestamp { get; set; }
        //public DateTime End { get; set; }

        public UserActivityQuery(
            string userId,
            DateTime timestamp
            //DateTime end
        )
        {
            UserId = userId;
            Timestamp = timestamp;
            //End = end;
        }
    }
}
