using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Templates.Api.Application.Dtos;
using Templates.Api.Application.Services;
using Templates.Api.Controllers;
using Templates.Api.Infrastructure.Responses;

namespace Templates.Api.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUsersService> _serviceMock;
        private readonly Mock<ILogger<UsersController>> _loggerMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _serviceMock = new Mock<IUsersService>();
            _loggerMock = new Mock<ILogger<UsersController>>();
            _controller = new UsersController(_serviceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            _serviceMock.Setup(x => x.GetUserByIdAsync(It.IsAny<int>(), default))
                        .ReturnsAsync((UserDto)null);

            var result = await _controller.GetUserById(1, default);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CreateUser_ShouldReturnCreatedUser()
        {
            var dto = new UserCreateDto { FirstName = "Alex", LastName = "Silveira" };
            var userDto = new UserDto { Id = 1, FirstName = "Alex", LastName = "Silveira" };

            _serviceMock.Setup(x => x.CreateUserAsync(dto, default)).ReturnsAsync(userDto);

            var result = await _controller.CreateUser(dto, default);

            var createdResult = result.Result as CreatedAtActionResult;

            createdResult.Should().NotBeNull();

            var apiResponse = (ApiResponse<UserDto>)(createdResult?.Value ?? new UserDto());

            apiResponse.Data?.FirstName.Should().Be("Alex");
        }
    }
}
