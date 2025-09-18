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
    public class TemplatesServiceTests
    {
        private readonly Mock<ITemplatesRepository> _templatesRepoMock;
        private readonly Mock<IUsersRepository> _usersRepoMock;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<TemplatesService>> _loggerMock;
        private readonly TemplatesService _service;

        public TemplatesServiceTests()
        {
            _templatesRepoMock = new Mock<ITemplatesRepository>();
            _usersRepoMock = new Mock<IUsersRepository>();
            _mapper = AutoMapperFixture.GetMapper();
            _loggerMock = new Mock<ILogger<TemplatesService>>();
            _service = new TemplatesService(_templatesRepoMock.Object, _usersRepoMock.Object, _mapper, _loggerMock.Object);
        }

        [Fact]
        public async Task GetTemplatesAsync_ShouldReturnAllTemplates()
        {
            var templates = new List<Template> { new Template { Id = 1, Value = "Test" } };
            _templatesRepoMock.Setup(x => x.GetAllAsync(default)).ReturnsAsync(templates);

            var result = await _service.GetTemplatesAsync(default);

            result.Should().HaveCount(1);
            result.First().Id.Should().Be(1);
        }

        [Fact]
        public async Task GetTemplateByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _templatesRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), default)).ReturnsAsync((Template?)null);

            var result = await _service.GetTemplateByIdAsync(1, default);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetTemplateByIdAsync_ShouldThrow_WhenIdIsZeroOrNegative()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _service.GetTemplateByIdAsync(0, default));
        }

        [Fact]
        public async Task CreateTemplateAsync_ShouldReturnCreatedTemplate()
        {
            var dto = new TemplateCreateDto { Value = "Hello {{ user.name }}" };
            _templatesRepoMock.Setup(x => x.AddAsync(It.IsAny<Template>(), default)).Returns(Task.CompletedTask);

            var result = await _service.CreateTemplateAsync(dto, default);

            result.Should().NotBeNull();
            result.Value.Should().Contain("Hello");
            _templatesRepoMock.Verify(x => x.AddAsync(It.IsAny<Template>(), default), Times.Once);
        }

        [Fact]
        public async Task CreateTemplateAsync_ShouldThrow_WhenTemplateInvalid()
        {
            var dto = new TemplateCreateDto { Value = "{{{ invalid template" };

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateTemplateAsync(dto, default));
        }

        [Fact]
        public async Task UpdateTemplateAsync_ShouldReturnNull_WhenTemplateNotFound()
        {
            var dto = new TemplateUpdateDto { Id = 1, Value = "Test" };
            _templatesRepoMock.Setup(x => x.GetByIdAsync(dto.Id, default)).ReturnsAsync((Template?)null);

            var result = await _service.UpdateTemplateAsync(dto, default);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteTemplateAsync_ShouldReturnTrue_WhenDeleted()
        {
            var template = new Template { Id = 1 };
            _templatesRepoMock.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(template);
            _templatesRepoMock.Setup(x => x.DeleteAsync(template, default)).Returns(Task.CompletedTask);

            var result = await _service.DeleteTemplateAsync(1, default);

            result.Should().BeTrue();
            _templatesRepoMock.Verify(x => x.DeleteAsync(template, default), Times.Once);
        }

        [Fact]
        public async Task GetTemplateByUserIdAsync_ShouldReturnRenderedTemplate()
        {
            var template = new Template { Id = 1, Value = "Hello {{ user.name }}" };
            var user = new User { Id = 2, FirstName = "Alex", LastName = "Silveira" };

            _templatesRepoMock.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(template);
            _usersRepoMock.Setup(x => x.GetByIdAsync(2, default)).ReturnsAsync(user);

            var result = await _service.GetTemplateByUserIdAsync(1, 2, default);

            result.Should().Contain("Hello Alex");
        }

        [Fact]
        public async Task GetTemplateByUserIdHtmlAsync_ShouldReturnHtml()
        {
            var template = new Template { Id = 1, Value = "Hello {{ user.name }}" };
            var user = new User { Id = 2, FirstName = "Alex", LastName = "Silveira" };

            _templatesRepoMock.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(template);
            _usersRepoMock.Setup(x => x.GetByIdAsync(2, default)).ReturnsAsync(user);

            var result = await _service.GetTemplateByUserIdHtmlAsync(1, 2, default);

            result.Should().Contain("<html>");
            result.Should().Contain("Hello Alex");
        }
    }
}
