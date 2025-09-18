using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
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
        private readonly Mock<ILogger<UsersService>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly UsersService _service;

        public UsersServiceTests()
        {
            _usersRepoMock = new Mock<IUsersRepository>();
            _mapper = AutoMapperFixture.GetMapper();
            _loggerMock = new Mock<ILogger<UsersService>>();
            _service = new UsersService(_usersRepoMock.Object, _mapper, _loggerMock.Object);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnAllUsers()
        {
            var users = new List<User>
            {
                new User { Id = 1, FirstName = "Alexsandro", LastName = "Silveira" },
                new User { Id = 2, FirstName = "Alex", LastName = "Silveira" }
            };

            _usersRepoMock.Setup(x => x.GetAllAsync(default)).ReturnsAsync(users);

            var result = await _service.GetUsersAsync(default);

            result.Should().HaveCount(2);
            result.First().LastName.Should().Be("Silveira");
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _usersRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), default))
                .ReturnsAsync((User?)null);

            var result = await _service.GetUserByIdAsync(10, default);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenExists()
        {
            var user = new User { Id = 5, FirstName = "Maria", LastName = "Silva" };
            _usersRepoMock.Setup(x => x.GetByIdAsync(5, default)).ReturnsAsync(user);

            var result = await _service.GetUserByIdAsync(5, default);

            result.Should().NotBeNull();
            result!.FirstName.Should().Be("Maria");
            result.LastName.Should().Be("Silva");
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnCreatedUser()
        {
            var dto = new UserCreateDto { FirstName = "Alex", LastName = "Silveira" };

            var result = await _service.CreateUserAsync(dto, default);

            result.FirstName.Should().Be("Alex");
            _usersRepoMock.Verify(x => x.AddAsync(It.IsAny<User>(), default), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnNull_WhenUserNotFound()
        {
            var dto = new UserUpdateDto { Id = 1, FirstName = "Updated" };
            _usersRepoMock.Setup(x => x.GetByIdAsync(dto.Id, default))
                .ReturnsAsync((User?)null);

            var result = await _service.UpdateUserAsync(dto, default);

            result.Should().BeNull();
            _usersRepoMock.Verify(x => x.UpdateAsync(It.IsAny<User>(), default), Times.Never);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnUser_WhenExists()
        {
            var user = new User { Id = 1, FirstName = "Old", LastName = "Name" };
            var dto = new UserUpdateDto { Id = 1, FirstName = "New", LastName = "Name" };

            _usersRepoMock.Setup(x => x.GetByIdAsync(dto.Id, default)).ReturnsAsync(user);

            var result = await _service.UpdateUserAsync(dto, default);

            result?.FirstName.Should().Be("New");
            _usersRepoMock.Verify(x => x.UpdateAsync(It.Is<User>(u => u.FirstName == "New"), default), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFalse_WhenNotFound()
        {
            _usersRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), default))
                .ReturnsAsync((User?)null);

            var result = await _service.DeleteUserAsync(99, default);

            result.Should().BeFalse();
            _usersRepoMock.Verify(x => x.DeleteAsync(It.IsAny<User>(), default), Times.Never);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserExists()
        {
            var user = new User { Id = 2, FirstName = "ToDelete" };
            _usersRepoMock.Setup(x => x.GetByIdAsync(2, default)).ReturnsAsync(user);

            var result = await _service.DeleteUserAsync(2, default);

            result.Should().BeTrue();
            _usersRepoMock.Verify(x => x.DeleteAsync(It.Is<User>(u => u.Id == 2), default), Times.Once);
        }
    }
}
