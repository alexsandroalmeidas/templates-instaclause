using AutoMapper;
using Templates.Api.Application.Dtos;
using Templates.Api.Data.Entities;
using Templates.Api.Data.Repositories;

namespace Templates.Api.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IRepository<User> _usersRepository;
        private readonly IMapper _mapper;

        public UsersService(IRepository<User> usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken cancellationToken)
        {
            var users = await _usersRepository.GetAllAsync(cancellationToken);

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.GetByIdAsync(id, cancellationToken);

            return user == null
                    ? null
                    : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto dto, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(dto);

            await _usersRepository.AddAsync(user, cancellationToken);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateUserAsync(UserUpdateDto dto, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.GetByIdAsync(dto.Id, cancellationToken);

            if (user == null) return false;

            _mapper.Map(dto, user);

            await _usersRepository.UpdateAsync(user, cancellationToken);

            return true;
        }

        public async Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.GetByIdAsync(id, cancellationToken);

            if (user == null) return false;

            await _usersRepository.DeleteAsync(user, cancellationToken);

            return true;
        }
    }
}
