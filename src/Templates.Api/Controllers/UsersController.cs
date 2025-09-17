using Microsoft.AspNetCore.Mvc;
using Templates.Api.Application.Dtos;
using Templates.Api.Application.Services;
using Templates.Api.Data;
using Templates.Api.Data.Entities;

namespace Templates.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService, AppDbContext context)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        /// <summary>
        /// Retorna uma lista de todos os usuários
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<User>))]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var users = await _usersService.GetUsersAsync(cancellationToken);
            return Ok(users);
        }

        /// <summary>
        /// Retorna um usuário pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken)
        {
            var user = await _usersService.GetUserAsync(id, cancellationToken);
            if (user == null) return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdUser = await _usersService.CreateUserAsync(dto, cancellationToken);

            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.Id) return BadRequest("O ID da URL não corresponde ao payload.");

            var result = await _usersService.UpdateUserAsync(dto, cancellationToken);
            if (!result) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Exclui um usuário pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
        {
            var deleted = await _usersService.DeleteUserAsync(id, cancellationToken);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
