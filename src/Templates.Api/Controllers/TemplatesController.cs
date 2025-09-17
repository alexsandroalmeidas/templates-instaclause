using Microsoft.AspNetCore.Mvc;
using Templates.Api.Application.Dtos;
using Templates.Api.Application.Services;

namespace Templates.Api.Controllers
{
    [ApiController]
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

        /// <summary>
        /// Retorna uma lista de todos os templates
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TemplateDto>))]
        public async Task<IActionResult> GetTemplates(CancellationToken cancellationToken)
        {
            var templates = await _templatesService.GetTemplatesAsync(cancellationToken);
            return Ok(templates);
        }

        /// <summary>
        /// Retorna um template pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TemplateDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTemplateById(int id, CancellationToken cancellationToken)
        {
            var template = await _templatesService.GetTemplateByIdAsync(id, cancellationToken);
            return template is null ? NotFound() : Ok(template);
        }

        /// <summary>
        /// Retorna um template pelo ID e usuário ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/compile/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TemplateDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTemplateByUserId(int id, int userId, CancellationToken cancellationToken)
        {
            var template = await _templatesService.GetTemplateByUserIdAsync(id, userId, cancellationToken);
            return template is null ? NotFound() : Ok(template);
        }

        /// <summary>
        /// Cria um novo template
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TemplateDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTemplate([FromBody] TemplateCreateDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdTemplate = await _templatesService.CreateTemplateAsync(dto, cancellationToken);

            _logger.LogInformation("Template criado com sucesso. Id: {TemplateId}", createdTemplate.Id);

            return CreatedAtAction(nameof(GetTemplateById), new { id = createdTemplate.Id }, createdTemplate);
        }

        /// <summary>
        /// Atualiza um template existente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTemplate(int id, [FromBody] TemplateUpdateDto dto, CancellationToken cancellationToken)
        {
            if (id != dto.Id)
                return BadRequest("O ID da URL não corresponde ao payload.");

            var updated = await _templatesService.UpdateTemplateAsync(dto, cancellationToken);

            if (!updated)
                return NotFound();

            _logger.LogInformation("Template atualizado com sucesso. Id: {TemplateId}", id);

            return NoContent();
        }

        /// <summary>
        /// Exclui um template pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTemplate(int id, CancellationToken cancellationToken)
        {
            var deleted = await _templatesService.DeleteTemplateAsync(id, cancellationToken);

            if (!deleted)
                return NotFound();

            _logger.LogInformation("Template excluído com sucesso. Id: {TemplateId}", id);

            return NoContent();
        }
    }
}
