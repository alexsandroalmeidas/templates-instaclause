using Templates.Api.Application.Dtos;

namespace Templates.Api.Application.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken cancellationToken);
        Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        Task<UserDto> CreateUserAsync(UserCreateDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateUserAsync(UserUpdateDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken);
    }
}
