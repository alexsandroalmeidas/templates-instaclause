using AutoMapper;
using FluentAssertions;
using Moq;
using Templates.Api.Application.Dtos;
using Templates.Api.Application.Services;
using Templates.Api.Data.Entities;
using Templates.Api.Data.Repositories;
using Templates.Api.Tests.TestHelpers;

namespace Templates.Api.Tests.Services
{
    public class UsersServiceTests
    {
        private readonly Mock<IUsersRepository> _usersRepoMock;
        private readonly IMapper _mapper;
        private readonly UsersService _service;

        public UsersServiceTests()
        {
            _usersRepoMock = new Mock<IUsersRepository>();
            _mapper = AutoMapperFixture.GetMapper();
            _service = new UsersService(_usersRepoMock.Object, _mapper);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnAllUsers()
        {
            var users = new List<User>
            {
                new User { Id = 1, FirstName = "John", LastName = "Doe" },
                new User { Id = 2, FirstName = "Jane", LastName = "Smith" }
            };

            _usersRepoMock.Setup(x => x.GetAllAsync(default)).ReturnsAsync(users);

            var result = await _service.GetUsersAsync(default);

            result.Should().HaveCount(2);
            result.First().FirstName.Should().Be("John");
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _usersRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), default)).ReturnsAsync((User)null);

            var result = await _service.GetUserByIdAsync(10, default);

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnCreatedUser()
        {
            var dto = new UserCreateDto { FirstName = "Alex", LastName = "Silveira" };

            var result = await _service.CreateUserAsync(dto, default);

            result.FirstName.Should().Be("Alex");
            _usersRepoMock.Verify(x => x.AddAsync(It.IsAny<User>(), default), Times.Once);
        }
    }
}
