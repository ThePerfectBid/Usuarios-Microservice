using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Usuarios.Application.Commands;
using Usuarios.Application.DTOs;
using Usuarios.Application.Events;
using Usuarios.Domain.Aggregates;
using Usuarios.Infrastructure.Queries;

namespace Usuarios.Presentation.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;

        public UsersController(IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        #region CreateUser
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {

            var userId = await _mediator.Send(new CreateUserCommand(userDto));
            if (userId == null)
            {
                return BadRequest("No se pudo crear el usuario.");
            }
            return CreatedAtAction(nameof(CreateUser), new { id = userId }, new
            {
                id = userId
                //message = "Usuario creado exitosamente."
            });
        }
        #endregion

        #region UpdateUserById
        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UpdateUserDto userDto)
        {

            var result = await _mediator.Send(new UpdateUserCommand(userDto, id));
            if (!result)
            {
                return NotFound("El usuario no pudo ser actualizado.");
            }

            return Ok("Usuario actualizado exitosamente.");
        }
        #endregion

        #region GetUserActivityById
        [HttpGet("{UserId}/activity")]
        public async Task<IActionResult> GetByUserIdAsync([FromRoute] string UserId) //, [FromQuery] DateTime start,
                                                                                     // [FromQuery] DateTime end
        {
            var activities = await _mediator.Send(new UserActivityQuery(UserId, DateTime.MaxValue));
            return Ok(activities);
        }
        #endregion

        #region CreateUserActivity
        [HttpPost("publishActivity")]
        public async Task<IActionResult> PublishUserActivity([FromBody] CreateUserActivityDto createuserActivityDto)
        {
            await _publishEndpoint.Publish(new UserActivityMadeEvent(createuserActivityDto.UserId, createuserActivityDto.Action, DateTime.UtcNow));
            return Ok("Evento de actividad publicado correctamente.");
        }
        #endregion

        #region GetUserByEmail

        [HttpGet("getuserbyemail")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            var query = new GetUserByEmailQuery(email);
            var userDto = await _mediator.Send(query);
            return Ok(userDto);
        }

        #endregion

    }

}
