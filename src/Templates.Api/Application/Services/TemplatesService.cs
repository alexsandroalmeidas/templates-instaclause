using AutoMapper;
using Scriban.Runtime;
using Templates.Api.Application.Dtos;
using Templates.Api.Data.Entities;
using Templates.Api.Data.Repositories;

namespace Templates.Api.Application.Services
{
    public class TemplatesService : ITemplatesService
    {
        private readonly ITemplatesRepository _templatesRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TemplatesService> _logger;

        private const string TemplateCssDefault = @"
                        h1 {
                            font-family: 'Arial';
                            font-size: 16pt;
                            font-weight: 700;
                            text-decoration: underline;
                            line-height: 1.15em;
                        }

                        h2 {
                            font-family: 'Arial';
                            font-size: 12pt;
                            font-weight: 700;
                            line-height: 1.15em;
                        }

                        h3 {
                            font-size: 10pt;
                            font-weight: 700;
                            line-height: 1.15em;
                        }

                        p {
                            font-size: 10pt;
                            font-weight: 400;
                            line-height: 1.5em;
                        }
        ";

        public TemplatesService(
            ITemplatesRepository templatesRepository,
            IUsersRepository usersRepository,
            IMapper mapper,
            ILogger<TemplatesService> logger)
        {
            _templatesRepository = templatesRepository ?? throw new ArgumentNullException(nameof(templatesRepository));
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<TemplateDto>> GetTemplatesAsync(CancellationToken cancellationToken)
        {
            var templates = await _templatesRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TemplateDto>>(templates);
        }

        public async Task<TemplateDto?> GetTemplateByIdAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "The Id must be greater than zero");

            var template = await _templatesRepository.GetByIdAsync(id, cancellationToken);
            return template == null ? null : _mapper.Map<TemplateDto>(template);
        }

        public async Task<string?> GetTemplateByUserIdHtmlAsync(int id, int userId, CancellationToken cancellationToken)
        {
            var renderTemplate = await GetRenderTemplateAsync(id, userId, cancellationToken);

            return string.IsNullOrWhiteSpace(renderTemplate)
                ? null
                : BuildHtml(renderTemplate);
        }

        public async Task<string?> GetTemplateByUserIdAsync(int id, int userId, CancellationToken cancellationToken)
            => await GetRenderTemplateAsync(id, userId, cancellationToken);

        private async Task<string?> GetRenderTemplateAsync(int templateId, int userId, CancellationToken cancellationToken)
        {
            if (templateId <= 0)
                throw new ArgumentOutOfRangeException(nameof(templateId), "The TemplateId must be greater than zero");

            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), "The UserId must be greater than zero");

            var template = await _templatesRepository.GetByIdAsync(templateId, cancellationToken);
            if (template == null)
            {
                _logger.LogWarning("Template not found. TemplateId: {TemplateId}", templateId);
                return null;
            }

            var user = await _usersRepository.GetByIdAsync(userId, cancellationToken)
                ?? throw new KeyNotFoundException($"User not found. UserId: {userId}");

            var userModel = BuildUserModel(user);

            return RenderTemplate(template.Value, userModel, "user");
        }

        private static object BuildUserModel(User user)
        {
            return new
            {
                Name = user.FirstName,
                LastName = user.LastName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                City = user.Address?.City ?? string.Empty,
                Country = user.Address?.Country ?? string.Empty,
                Street = user.Address?.Street ?? string.Empty,
                HouseNumber = user.Address?.HouseNumber ?? string.Empty,
                State = user.Address?.State ?? string.Empty,
                ZipCode = user.Address?.ZipCode ?? string.Empty
            };
        }

        private static string BuildHtml(string content)
        {
            return @$"
                <!DOCTYPE html>
                <html>
                    <head>
                      <style>
                        {TemplateCssDefault}
                      </style>
                    </head>
                    <body>
                        {content}
                    </body>
                </html>";
        }

        private static string RenderTemplate(string templateText, object model, string variableName = "user")
        {
            var template = Scriban.Template.Parse(templateText);

            if (template.HasErrors)
                throw new InvalidOperationException($"Template errors: {string.Join(", ", template.Messages)}");

            var scriptObject = new ScriptObject { [variableName] = model };
            var context = new Scriban.TemplateContext();
            context.PushGlobal(scriptObject);

            return template.Render(context);
        }

        public async Task<TemplateDto> CreateTemplateAsync(TemplateCreateDto dto, CancellationToken cancellationToken)
        {
            ValidateTemplate(dto.Value);

            var template = _mapper.Map<Template>(dto);
            await _templatesRepository.AddAsync(template, cancellationToken);

            _logger.LogInformation("Template created successfully. TemplateId: {TemplateId}", template.Id);

            return _mapper.Map<TemplateDto>(template);
        }

        private void ValidateTemplate(string input)
        {
            var templateParse = Scriban.Template.Parse(input);

            if (templateParse.HasErrors)
            {
                var msgError = $"Invalid template. Errors: {string.Join(", ", templateParse.Messages)}";
                _logger.LogWarning(msgError);
                throw new InvalidOperationException(msgError);
            }

            _logger.LogInformation("Template validated successfully");
        }

        public async Task<TemplateDto?> UpdateTemplateAsync(TemplateUpdateDto dto, CancellationToken cancellationToken)
        {
            ValidateTemplate(dto.Value);

            var template = await _templatesRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (template == null)
            {
                _logger.LogWarning("Template not found for update. TemplateId: {TemplateId}", dto.Id);
                return null;
            }

            _mapper.Map(dto, template);

            await _templatesRepository.UpdateAsync(template, cancellationToken);

            _logger.LogInformation("Template updated successfully. TemplateId: {TemplateId}", dto.Id);

            return _mapper.Map<TemplateDto>(template);
        }

        public async Task<bool> DeleteTemplateAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "The Id must be greater than zero");

            var template = await _templatesRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new KeyNotFoundException($"Template not found. TemplateId: {id}");

            await _templatesRepository.DeleteAsync(template, cancellationToken);

            _logger.LogInformation("Template deleted successfully. TemplateId: {TemplateId}", id);
            return true;
        }
    }
}
