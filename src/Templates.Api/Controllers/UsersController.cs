using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Templates.Api.Application.Dtos;
using Templates.Api.Application.Services;
using Templates.Api.Data.Entities;
using Templates.Api.Infrastructure.Responses;

namespace Templates.Api.Controllers
{
    [ApiController]
    //[ApiVersion("1.0")]
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
        /// Retorna uma lista paginada de usuários
        /// </summary>
        [HttpGet]
        //[Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<UserDto>>))]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetUsers(CancellationToken cancellationToken = default)
        {
            var users = await _usersService.GetUsersAsync(cancellationToken);

            return Ok(ApiResponse<IEnumerable<UserDto>>.Ok(users));
        }

        /// <summary>
        /// Retorna um usuário pelo ID
        /// </summary>
        [HttpGet("{id:int}")]
        //[Authorize(Roles = "Admin,Manager,User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<UserDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(int id, CancellationToken cancellationToken)
        {
            var user = await _usersService.GetUserByIdAsync(id, cancellationToken);
            return user is null
                ? NotFound(ApiResponse<UserDto>.Fail(new[] { "User not found." }))
                : Ok(ApiResponse<UserDto>.Ok(user));
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<UserDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<UserDto>>> CreateUser(
            [FromBody] UserCreateDto dto,
            CancellationToken cancellationToken)
        {
            var createdUser = await _usersService.CreateUserAsync(dto, cancellationToken);

            _logger.LogInformation("User created successfully. Id: {UserId}", createdUser.Id);

            return CreatedAtAction(nameof(GetUserById),
                //new { id = createdUser.Id, version = "1.0" },
                new { id = createdUser.Id },
                ApiResponse<UserDto>.Ok(createdUser, "User created successfully"));
        }

        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        [HttpPut("{id:int}")]
        //[Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(
            int id,
            [FromBody] UserUpdateDto dto,
            CancellationToken cancellationToken)
        {
            if (id != dto.Id)
                return BadRequest(ApiResponse<UserDto>.Fail(new[] { "The URL ID does not match the payload ID." }));

            var updatedUser = await _usersService.UpdateUserAsync(dto, cancellationToken);

            if (updatedUser == null)
                return NotFound(ApiResponse<UserDto>.Fail(new[] { "User not found for update." }));

            _logger.LogInformation("User updated successfully. Id: {UserId}", id);

            return Ok(ApiResponse<UserDto>.Ok(updatedUser, "User updated successfully"));
        }

        /// <summary>
        /// Exclui um usuário pelo ID
        /// </summary>
        [HttpDelete("{id:int}")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
        {
            var deleted = await _usersService.DeleteUserAsync(id, cancellationToken);

            if (!deleted)
                return NotFound(ApiResponse<UserDto>.Fail(new[] { "User not found for deletion." }));

            _logger.LogInformation("User deleted successfully. Id: {UserId}", id);

            return NoContent();
        }
    }
}
