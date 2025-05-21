using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Usuarios.Application.DTOs;
using Usuarios.Domain.Repositories;
using Usuarios.Infrastructure.Interfaces;

namespace Usuarios.Infrastructure.Queries.QueryHandlers
{
    public class UserActivityQueryHandler : IRequestHandler<UserActivityQuery, IEnumerable<UserActivityDto>>
    {
        private readonly IUserActivityReadRepository _userActivityReadRepository;

        public UserActivityQueryHandler(IUserActivityReadRepository userActivityReadRepository)
        {
            _userActivityReadRepository = userActivityReadRepository;
        }

        public async Task<IEnumerable<UserActivityDto>> Handle(UserActivityQuery request, CancellationToken cancellationToken)
        {
            var activities = await _userActivityReadRepository.GetByUserIdAsync(request.UserId, DateTime.Today); //, request.Start, request.End

            //if (activities == null || activities.Count == 0)
            //    return new List<UserActivityDto>(); //Retorna una lista vacía si no hay actividad

            return activities.ConvertAll(activity => new UserActivityDto
            {
                Id = activity["_id"].AsString,
                UserId = activity["_userid"].AsString,
                Action = activity["message"].AsString,
                Timestamp = activity["timestamp"].AsLocalTime
            });
        }
    }
}
