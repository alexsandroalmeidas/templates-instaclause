using Microsoft.AspNetCore.Mvc;
using Templates.Api.Application.Dtos;
using Templates.Api.Application.Services;

namespace Templates.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUsersService usersService,
            ILogger<UsersController> logger)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retorna uma lista de todos os usuários
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDto>))]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(CancellationToken cancellationToken)
        {
            var users = await _usersService.GetUsersAsync(cancellationToken);
            return Ok(users);
        }

        /// <summary>
        /// Retorna um usuário pelo ID
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUserById(int id, CancellationToken cancellationToken)
        {
            var user = await _usersService.GetUserByIdAsync(id, cancellationToken);
            return user is null ? NotFound() : Ok(user);
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserCreateDto dto, CancellationToken cancellationToken)
        {
            var createdUser = await _usersService.CreateUserAsync(dto, cancellationToken);

            _logger.LogInformation("Usuário criado com sucesso. Id: {UserId}", createdUser.Id);

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto dto, CancellationToken cancellationToken)
        {
            if (id != dto.Id)
                return BadRequest("O ID da URL não corresponde ao payload.");

            var updated = await _usersService.UpdateUserAsync(dto, cancellationToken);
            if (!updated)
                return NotFound();

            _logger.LogInformation("Usuário atualizado com sucesso. Id: {UserId}", id);

            return NoContent();
        }

        /// <summary>
        /// Exclui um usuário pelo ID
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
        {
            var deleted = await _usersService.DeleteUserAsync(id, cancellationToken);
            if (!deleted)
                return NotFound();

            _logger.LogInformation("Usuário excluído com sucesso. Id: {UserId}", id);

            return NoContent();
        }
    }
}