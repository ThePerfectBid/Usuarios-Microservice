using MediatR;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Application.Commands;
using Usuarios.Application.DTOs;

namespace Usuarios.Presentation.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            // Validar unicidad antes de enviar el comando

            var userId = await _mediator.Send(new CreateUserCommand(userDto));
            return CreatedAtAction(nameof(CreateUser), new { id = userId });
        }

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
    }
}
