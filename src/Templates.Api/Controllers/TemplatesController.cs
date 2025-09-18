using Microsoft.AspNetCore.Mvc;
using System.Text;
using Templates.Api.Application.Dtos;
using Templates.Api.Application.Services;
using Templates.Api.Infrastructure.Responses;

namespace Templates.Api.Controllers
{
    [ApiController]
    //[ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class TemplatesController : ControllerBase
    {
        private readonly ITemplatesService _templatesService;
        private readonly ILogger<TemplatesController> _logger;

        public TemplatesController(
            ITemplatesService templatesService,
            ILogger<TemplatesController> logger)
        {
            _templatesService = templatesService ?? throw new ArgumentNullException(nameof(templatesService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<TemplateDto>>))]
        public async Task<IActionResult> GetTemplates(CancellationToken cancellationToken)
        {
            var templates = await _templatesService.GetTemplatesAsync(cancellationToken);
            return Ok(ApiResponse<IEnumerable<TemplateDto>>.Ok(templates));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TemplateDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetTemplateById(int id, CancellationToken cancellationToken)
        {
            var template = await _templatesService.GetTemplateByIdAsync(id, cancellationToken);
            return template is null
                ? NotFound(ApiResponse<string>.Fail($"Template {id} not found"))
                : Ok(ApiResponse<TemplateDto>.Ok(template));
        }

        [HttpGet("{id:int}/compile/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetTemplateByUserId(int id, int userId, CancellationToken cancellationToken)
        {
            var template = await _templatesService.GetTemplateByUserIdAsync(id, userId, cancellationToken);
            return template is null
                ? NotFound(ApiResponse<string>.Fail($"Template {id} not found for User {userId}"))
                : Ok(ApiResponse<string>.Ok(template));
        }

        [HttpGet("{id:int}/compile/{userId:int}/html")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetTemplateByUserIdHtml(int id, int userId, CancellationToken cancellationToken)
        {
            var templateHtml = await _templatesService.GetTemplateByUserIdHtmlAsync(id, userId, cancellationToken);

            if (templateHtml is null)
                return NotFound(ApiResponse<string>.Fail($"Template {id} not found for User {userId}"));

            return Content(templateHtml, "text/html", Encoding.UTF8);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<TemplateDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<string>))]
        public async Task<IActionResult> CreateTemplate([FromBody] TemplateCreateDto dto, CancellationToken cancellationToken)
        {
            var createdTemplate = await _templatesService.CreateTemplateAsync(dto, cancellationToken);

            return CreatedAtAction(nameof(GetTemplateById), new { id = createdTemplate.Id },
                ApiResponse<TemplateDto>.Ok(createdTemplate, "Template created successfully"));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<TemplateDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<TemplateDto>))]
        public async Task<IActionResult> UpdateTemplate(int id, [FromBody] TemplateUpdateDto dto, CancellationToken cancellationToken)
        {
            if (id != dto.Id)
                return BadRequest(ApiResponse<string>.Fail("The Id in URL does not match the payload."));

            var updatedTemplate = await _templatesService.UpdateTemplateAsync(dto, cancellationToken);

            if (updatedTemplate == null)
                return NotFound(ApiResponse<string>.Fail($"Template {id} not found"));

            return Ok(ApiResponse<TemplateDto>.Ok(updatedTemplate, "Template updated successfully"));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>))]
        public async Task<IActionResult> DeleteTemplate(int id, CancellationToken cancellationToken)
        {
            var deleted = await _templatesService.DeleteTemplateAsync(id, cancellationToken);

            if (!deleted)
                return NotFound(ApiResponse<string>.Fail($"Template {id} not found"));

            _logger.LogInformation("Template deleted successfully. Id: {TemplateId}", id);

            return NoContent();
        }
    }
}
