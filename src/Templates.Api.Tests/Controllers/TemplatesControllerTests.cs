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
    public class TemplatesControllerTests
    {
        private readonly Mock<ITemplatesService> _serviceMock;
        private readonly Mock<ILogger<TemplatesController>> _loggerMock;
        private readonly TemplatesController _controller;

        public TemplatesControllerTests()
        {
            _serviceMock = new Mock<ITemplatesService>();
            _loggerMock = new Mock<ILogger<TemplatesController>>();
            _controller = new TemplatesController(_serviceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetTemplates_ShouldReturnOkWithTemplates()
        {
            var templates = new List<TemplateDto> { new TemplateDto { Id = 1, Value = "Test" } };
            _serviceMock.Setup(x => x.GetTemplatesAsync(default)).ReturnsAsync(templates);

            var result = await _controller.GetTemplates(default);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var apiResponse = (ApiResponse<IEnumerable<TemplateDto>>)(okResult?.Value ?? Enumerable.Empty<TemplateDto>());
            apiResponse.Data.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetTemplateById_ShouldReturnNotFound_WhenTemplateDoesNotExist()
        {
            _serviceMock.Setup(x => x.GetTemplateByIdAsync(1, default)).ReturnsAsync((TemplateDto)null);

            var result = await _controller.GetTemplateById(1, default);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CreateTemplate_ShouldReturnCreated()
        {
            var dto = new TemplateCreateDto { Value = "Hello {{ user.Name }}" };
            var templateDto = new TemplateDto { Id = 1, Value = "Hello {{ user.Name }}" };

            _serviceMock.Setup(x => x.CreateTemplateAsync(dto, default)).ReturnsAsync(templateDto);

            var result = await _controller.CreateTemplate(dto, default);

            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();

            var apiResponse = (ApiResponse<TemplateDto>)(createdResult?.Value ?? new TemplateDto());

            apiResponse.Data?.Id.Should().Be(1);
        }

        [Fact]
        public async Task UpdateTemplate_ShouldReturnBadRequest_WhenIdMismatch()
        {
            var dto = new TemplateUpdateDto { Id = 1, Value = "Hello" };
            var result = await _controller.UpdateTemplate(2, dto, default);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteTemplate_ShouldReturnNoContent_WhenDeleted()
        {
            _serviceMock.Setup(x => x.DeleteTemplateAsync(1, default)).ReturnsAsync(true);

            var result = await _controller.DeleteTemplate(1, default);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteTemplate_ShouldReturnNotFound_WhenNotDeleted()
        {
            _serviceMock.Setup(x => x.DeleteTemplateAsync(1, default)).ReturnsAsync(false);

            var result = await _controller.DeleteTemplate(1, default);

            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
