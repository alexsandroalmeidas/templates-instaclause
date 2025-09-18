using AutoMapper;
using Mono.TextTemplating;
using Scriban.Runtime;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
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
                throw new Exception("The Id must be greater than zero");

            var template = await _templatesRepository.GetByIdAsync(id, cancellationToken);

            return template == null
                    ? null
                    : _mapper.Map<TemplateDto>(template);
        }

        public async Task<TemplateDto?> GetTemplateByUserIdAsync(int id, int userId, CancellationToken cancellationToken)
        {
            if (id <= 0)
                throw new Exception("The Id must be greater than zero");

            if (userId <= 0)
                throw new Exception("The UserId must be greater than zero");

            var template = await _templatesRepository.GetByIdAsync(id, cancellationToken);

            if (template == null)
                throw new Exception($"Template ID {id} not found ");

            var user = await _usersRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null)
                throw new Exception($"User ID {userId} not found ");

            var userCast = new
            {
                Name = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                City = user.Address?.City ?? string.Empty,
                Country = user.Address?.Country ?? string.Empty,
                Street = user.Address?.Street ?? string.Empty,
                HouseNumber = user.Address?.HouseNumber ?? string.Empty,
                State = user.Address?.State ?? string.Empty,
                ZipCode = user.Address?.ZipCode ?? string.Empty
            };

            var result = RenderTemplate(template.Value, userCast, "user");

            return template == null
                    ? null
                    : _mapper.Map<TemplateDto>(template);
        }

        private static string RenderTemplate(string templateText, object model, string variableName = "user")
        {
            var template = Scriban.Template.Parse(templateText);
            if (template.HasErrors)
                throw new Exception($"Erros no template: {string.Join(", ", template.Messages)}");

            var scriptObject = new ScriptObject
            {
                [variableName] = model
            };

            var context = new Scriban.TemplateContext();
            context.PushGlobal(scriptObject);

            return template.Render(context);
        }

        public async Task<TemplateDto> CreateTemplateAsync(TemplateCreateDto dto, CancellationToken cancellationToken)
        {
            ValidateTemplate(dto.Value);

            var template = _mapper.Map<Template>(dto);

            await _templatesRepository.AddAsync(template, cancellationToken);

            return _mapper.Map<TemplateDto>(template);
        }

        private void ValidateTemplate(string input)
        {
            var templateParse = Scriban.Template.Parse(input);

            if (templateParse.HasErrors)
            {
                var msgError = "Template invalid!";
                _logger.LogInformation(msgError);
                throw new Exception(msgError);
            }
            else
            {
                _logger.LogInformation("Template valid!");
            }
        }

        public async Task<bool> UpdateTemplateAsync(TemplateUpdateDto dto, CancellationToken cancellationToken)
        {
            ValidateTemplate(dto.Value);

            var template = await _templatesRepository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new Exception($"Template {dto.Id} not found");

            _mapper.Map(dto, template);

            await _templatesRepository.UpdateAsync(template, cancellationToken);

            return true;
        }

        public async Task<bool> DeleteTemplateAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                throw new Exception("The Id must be greater than zero");

            var template = await _templatesRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new Exception($"Template {id} not found");

            await _templatesRepository.DeleteAsync(template, cancellationToken);

            return true;
        }
    }
}
